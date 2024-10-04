using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticleController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleController"/> class.
        /// </summary>
        /// <param name="articleService">Service for handling article operations.</param>
        /// <param name="logger">Logger for logging error or info messages.</param>
        public ArticleController(IArticleService articleService, ILogger<ArticleController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new article.
        /// </summary>
        /// <param name="articleForCreationDto">Data transfer object containing article creation data.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the creation operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleForCreationDto articleForCreationDto)
        {
            var username = GetUsername();
            if (username == null) return Unauthorized();

            try
            {
                var article = await _articleService.CreateArticleAsync(articleForCreationDto, username);
                return CreatedAtAction(nameof(CreateArticle), new { article.title }, article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating article");
                return StatusCode(500, "An error occurred while creating the article.");
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
            var username = GetUsername();
            if (username == null) return Unauthorized();

            try
            {
                var updatedArticle = await _articleService.EditArticleAsync(id, articleForUpdateDto, username);
                return Ok(updatedArticle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating article with ID {id}");
                return StatusCode(500, "An error occurred while updating the article.");
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
                var articleDetails = await _articleService.GetArticleByIdAsync(id);
                if (articleDetails == null) return NotFound("Article not found.");
                return Ok(articleDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching article with ID {id}");
                return StatusCode(500, "An error occurred while fetching the article details.");
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
            var username = GetUsername();
            if (username == null) return Unauthorized();

            try
            {
                var deleted = await _articleService.DeleteArticleAsync(id, username);
                if (!deleted) return BadRequest("Failed to delete the article.");
                return Ok(new { message = "Article deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting article with ID {id}");
                return StatusCode(500, "An error occurred while deleting the article.");
            }
        }

        /// <summary>
        /// Retrieves a paginated list of articles.
        /// </summary>
        /// <param name="offset">The page number for pagination.</param>
        /// <param name="keyword">Optional keyword to filter articles by title or content.</param>
        /// <param name="tag">Optional tag to filter articles by.</param>
        /// <returns>An <see cref="IActionResult"/> containing a list of articles.</returns>
        [HttpGet]
        public async Task<IActionResult> GetArticles([FromQuery] ArticlesSearchDto searchDto)
        {
            try
            {
                var articles = await _articleService.GetArticlesAsync(searchDto);
                if (!articles.Any()) return NotFound("No articles found.");
                return Ok(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching articles with offset: {searchDto.Offset}, keyword: {searchDto.keyword}, tag: {searchDto.tag}");
                return StatusCode(500, "An error occurred while fetching the articles.");
            }
        }
        /// <summary>
        /// Retrieves the current user's username from the claims.
        /// </summary>
        /// <returns>The username of the current user or null if not authenticated.</returns>
        private string GetUsername()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
