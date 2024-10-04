using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    /// <summary>
    /// Interface for managing article-related data operations.
    /// </summary>
    public interface IArticleRepository
    {
        /// <summary>
        /// Asynchronously adds a new article to the repository.
        /// </summary>
        /// <param name="article">The article entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(Article article);

        /// <summary>
        /// Asynchronously saves all changes made to the repository context.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with the number of affected rows as a result.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Updates an existing article in the repository.
        /// </summary>
        /// <param name="article">The article entity with updated values.</param>
        void Update(Article article);

        /// <summary>
        /// Asynchronously retrieves an article by its ID.
        /// </summary>
        /// <param name="articleId">The unique ID of the article.</param>
        /// <returns>A task that represents the asynchronous operation, containing the article entity if found.</returns>
        Task<Article> GetArticleDetailsAsync(int articleId);

        /// <summary>
        /// Deletes the specified article from the repository.
        /// </summary>
        /// <param name="article">The article entity to be deleted.</param>
        void Delete(Article article);

        /// <summary>
        /// Asynchronously retrieves a paginated list of articles based on the provided filters.
        /// </summary>
        /// <param name="offset">The offset for pagination (page number).</param>
        /// <param name="keyword">Optional keyword to search articles by title or body.</param>
        /// <param name="tag">Optional tag to filter articles by.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of articles matching the filters.</returns>
        Task<IEnumerable<Article>> GetArticlesAsync(int offset, string keyword, string tag);
    }
}
