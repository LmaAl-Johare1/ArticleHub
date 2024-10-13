using Core.IServices;
using Core.Models;
using Core.Services;
using Data.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// Controller for handling article-related actions.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/articles")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticleController> _logger;
        private readonly ArticleHubDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleController"/> class.
        /// </summary>
        /// <param name="articleService">Service for handling article operations.</param>
        /// <param name="logger">Logger for logging error or info messages.</param>
        public ArticleController(IArticleService articleService, ILogger<ArticleController> logger, ArticleHubDbContext context)
        {
            _articleService = articleService;
            _logger = logger;
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        /// <summary>
        /// Creates a new article.
        /// </summary>
        /// <param name="articleForCreationDto">Data transfer object containing article creation data.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the creation operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleForCreationDto articleForCreationDto)
        {
            var username = GetUsernameOrUnauthorized();
            if (username == null) return Unauthorized();

            try
            {
                var article = await _articleService.CreateArticleAsync(articleForCreationDto, username);
                if (article != null)
                {
                    return CreatedAtAction(nameof(GetArticle), new { title = article.title }, article);
                }
                return BadRequest("Failed to create article.");
            }
            catch (Exception ex)
            {
                LogError(ex, articleForCreationDto.title);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Edits an existing article.
        /// </summary>
        /// <param name="id">The ID of the article to edit.</param>
        /// <param name="articleForUpdateDto">Data transfer object containing updated article data.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditArticle(int id, [FromForm] ArticleForUpdateDto articleForUpdateDto)
        {
            var username = GetUsernameOrUnauthorized();
            if (username == null) return Unauthorized();

            try
            {
                var updatedArticle = await _articleService.EditArticleAsync(id, articleForUpdateDto, username);
                return Ok(updatedArticle);
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error updating article with ID {id}");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Gets an article by its ID.
        /// </summary>
        /// <param name="id">The ID of the article to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the article data if found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle(int id)
        {
            try
            {
                var article = await _articleService.GetArticleByIdAsync(id);
                if (article == null) return NotFound("Article not found.");
                return Ok(article);
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error fetching article with ID {id}");
                return StatusCode(500, "An error occurred while fetching the article.");
            }
        }

        /// <summary>
        /// Deletes an article by its ID.
        /// </summary>
        /// <param name="id">The ID of the article to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var username = GetUsernameOrUnauthorized();
            if (username == null) return Unauthorized();

            try
            {
                var deleted = await _articleService.DeleteArticleAsync(id, username);
                if (!deleted) return BadRequest("Failed to delete the article.");
                return Ok(new { message = "Article deleted successfully." });
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error deleting article with ID {id}");
                return StatusCode(500, "An error occurred while deleting the article.");
            }
        }

        /// <summary>
        /// Retrieves a paginated list of articles.
        /// </summary>
        /// <param name="searchDto">Data transfer object containing search parameters.</param>
        /// <returns>An <see cref="IActionResult"/> containing a list of articles.</returns>
        [HttpGet]
        public async Task<IActionResult> GetArticles([FromQuery] ArticlesSearchDto searchDto)
        {
            if (searchDto.Offset < 1)
            {
                searchDto.Offset = 1;
            }

            try
            {
                // Assuming GetArticlesAsync handles filtering by tag and keyword
                var articles = await _articleService.GetArticlesAsync(searchDto);
                var totalArticlesCount = await _articleService.GetTotalArticlesCount(searchDto.keyword, searchDto.tag); // Fetch total count

                if (!articles.Any()) return NotFound("No articles found.");
                return Ok(new { articles, totalCount = totalArticlesCount }); // Return articles and total count
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error fetching articles with offset: {searchDto.Offset}, keyword: {searchDto.keyword}, tag: {searchDto.tag}");
                return StatusCode(500, "An error occurred while fetching the articles.");
            }
        }

      

        /// <summary>
        /// Adds a comment to an article.
        /// </summary>
        /// <param name="articleId">ID of the article to comment on.</param>
        /// <param name="commentDto">Data transfer object containing comment information.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("{articleId}/comment")]
        public async Task<IActionResult> AddComment(int articleId, [FromBody] ArticleCommentDto commentDto)
        {
            var username = GetUsernameOrUnauthorized();
            if (username == null) return Unauthorized();

            try
            {
                var comment = await _articleService.AddCommentToArticleAsync(articleId, commentDto, username);
                return CreatedAtAction(nameof(GetArticle), new { id = articleId }, comment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error adding comment to article.");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Likes an article.
        /// </summary>
        /// <param name="slug">The slug of the article to like.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the result of the like operation.</returns>
        [HttpPost("{slug}/like")]
        public async Task<ActionResult> LikeArticle(string slug)
        {
            try
            {
                var likedArticle = await _articleService.LikeArticleAsync(slug);
                if (!likedArticle)
                {
                    return BadRequest("Failed to like the article.");
                }
                return Created("", new { article = likedArticle });
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error liking article with slug {slug}");
                return StatusCode(500, "An error occurred while liking the article.");
            }
        }

        /// <summary>
        /// Unlikes an article.
        /// </summary>
        /// <param name="slug">The slug of the article to unlike.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the result of the unlike operation.</returns>
        [HttpDelete("{slug}/like")]
        public async Task<ActionResult> UnLikeArticle(string slug)
        {
            try
            {
                var unlikedArticle = await _articleService.UnLikeArticleAsync(slug);
                if (!unlikedArticle)
                {
                    return BadRequest("Failed to unlike the article.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error unliking article with slug {slug}");
                return StatusCode(500, "An error occurred while unliking the article.");
            }
        }

        /// <summary>
        /// Retrieves the current user's username from the claims or returns Unauthorized.
        /// </summary>
        /// <returns>The username or null if not found.</returns>
        private string GetUsernameOrUnauthorized()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(username) ? null : username;
        }

        /// <summary>
        /// Logs errors with exception details.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="message">The custom message to log with the exception.</param>
        private void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }
    }
}
