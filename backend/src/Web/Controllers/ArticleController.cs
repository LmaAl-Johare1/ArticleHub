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
            var username = GetUsername();
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(); 
            }


            try
            {
                var result = await _articleService.CreateArticleAsync(articleForCreationDto, username);
                return CreatedAtAction(nameof(CreateArticle), new { title = result.title }, result);
            }
            catch (Exception ex)
            {
                LogError(ex, articleForCreationDto.title);
                return StatusCode(500, "Internal server error.");
            }
        }

        private string GetUsername()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private void LogError(Exception ex, string title)
        {
            _logger.LogError(ex, "Error creating article: {Title}", title);
        }
    }
}
