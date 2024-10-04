using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    /// <summary>
    /// Interface for managing article-related business logic.
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Asynchronously creates a new article.
        /// </summary>
        /// <param name="articleForCreationDto">The DTO containing article creation details.</param>
        /// <param name="username">The username of the user creating the article.</param>
        /// <returns>A task that represents the asynchronous operation, containing the created article DTO.</returns>
        Task<ArticleDto> CreateArticleAsync(ArticleForCreationDto articleForCreationDto, string username);

        /// <summary>
        /// Asynchronously edits an existing article.
        /// </summary>
        /// <param name="articleId">The ID of the article to edit.</param>
        /// <param name="articleForUpdateDto">The DTO containing updated article details.</param>
        /// <param name="username">The username of the user editing the article.</param>
        /// <returns>A task that represents the asynchronous operation, containing the updated article DTO.</returns>
        Task<ArticleDto> EditArticleAsync(int articleId, ArticleForUpdateDto articleForUpdateDto, string username);

        /// <summary>
        /// Asynchronously retrieves an article by its ID.
        /// </summary>
        /// <param name="articleId">The ID of the article to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing the requested article DTO.</returns>
        Task<ArticleCardDto> GetArticleByIdAsync(int articleId);

        /// <summary>
        /// Asynchronously deletes an article.
        /// </summary>
        /// <param name="articleId">The ID of the article to delete.</param>
        /// <param name="username">The username of the user requesting the deletion.</param>
        /// <returns>A task that represents the asynchronous operation, containing a boolean indicating success or failure.</returns>
        Task<bool> DeleteArticleAsync(int articleId, string username);

        /// <summary>
        /// Asynchronously retrieves a list of articles with optional filtering.
        /// </summary>
        /// <param name="offset">The offset for pagination (page number).</param>
        /// <param name="keyword">Optional keyword to search articles by title or body.</param>
        /// <param name="tag">Optional tag to filter articles by.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of article DTOs.</returns>
        Task<IEnumerable<ArticleDto>> GetArticlesAsync(ArticlesSearchDto articlesSearch);
    }
}
