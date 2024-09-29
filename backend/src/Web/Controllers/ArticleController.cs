using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IArticleService articleService, ILogger<ArticleController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleForCreationDto articleForCreationDto)
        {
            var usernameResult = GetUsernameOrUnauthorized();
            if (usernameResult is UnauthorizedResult)
            {
                return usernameResult;
            }

            var username = (string)((OkObjectResult)usernameResult).Value;

            try
            {
                var result = await _articleService.CreateArticleAsync(articleForCreationDto, username);
                if (result == null)
                {
                    return BadRequest("Failed to create article.");
                }
                return CreatedAtAction(nameof(CreateArticle), new { title = result.title }, result);
            }
            catch (Exception ex)
            {
                LogError(ex, articleForCreationDto.title);
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditArticle(int id, [FromForm] ArticleForUpdateDto articleForUpdateDto)
        {
            var usernameResult = GetUsernameOrUnauthorized();
            if (usernameResult is UnauthorizedResult)
            {
                return usernameResult;
            }

            var username = (string)((OkObjectResult)usernameResult).Value;

            try
            {
                var result = await _articleService.EditArticleAsync(id, articleForUpdateDto, username);
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error editing article: {id}");
                return StatusCode(500, "Internal server error.");
            }
        }

        private IActionResult GetUsernameOrUnauthorized()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            return Ok(username); 
        }

        private void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }
    }
}
