using Data.DbContexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    /// <summary>
    /// Repository for managing article data in the database.
    /// </summary>
    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleHubDbContext _context;
        private readonly ILogger<ArticleRepository> _logger;

        // Constant page size
        private const int PageSize = 6;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleRepository"/> class.
        /// </summary>
        /// <param name="context">The article hub database context.</param>
        /// <param name="logger">The logger for logging information and errors.</param>
        public ArticleRepository(ArticleHubDbContext context, ILogger<ArticleRepository> logger)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a new article to the database asynchronously.
        /// </summary>
        /// <param name="article">The article to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(Article article)
        {
            try
            {
                await _context.article.AddAsync(article);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding article: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Saves all changes made in the context to the database asynchronously.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving changes: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing article in the database.
        /// </summary>
        /// <param name="article">The article to update.</param>
        public void Update(Article article)
        {
            try
            {
                _context.article.Update(article);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating article: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves an article by its ID asynchronously.
        /// </summary>
        /// <param name="articleId">The ID of the article to retrieve.</param>
        /// <returns>The article entity if found, otherwise null.</returns>
        public async Task<Article> GetArticleDetailsAsync(int articleId)
        {
            try
            {
                return await _context.article
                    .Include(a => a.user) // Include the user who created the article
                    .Include(a => a.article_comments) // Include comments
                        .ThenInclude(c => c.user) // Include the user for comments
                    .Include(a => a.article_tags) // Include article tags
                        .ThenInclude(at => at.tag) // Include tags
                    .AsSplitQuery() // Use split query to improve performance
                    .FirstOrDefaultAsync(a => a.id == articleId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving article details by ID {articleId}: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes an article from the database.
        /// </summary>
        /// <param name="article">The article to delete.</param>
        public void Delete(Article article)
        {
            try
            {
                _context.article.Remove(article);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting article: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a paginated list of articles with optional keyword and tag filtering asynchronously.
        /// </summary>
        /// <param name="offset">The offset for pagination.</param>
        /// <param name="keyword">Optional keyword to filter articles by title or body.</param>
        /// <param name="tag">Optional tag to filter articles by associated tags.</param>
        /// <returns>A list of articles matching the filtering criteria.</returns>
        public async Task<IEnumerable<Article>> GetArticlesAsync(int offset, string keyword, string tag)
        {
            try
            {
                var query = _context.article
                    .Include(a => a.user)
                    .Include(a => a.article_tags)
                    .ThenInclude(at => at.tag)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(a => a.title.Contains(keyword) || a.body.Contains(keyword));
                }

                if (!string.IsNullOrEmpty(tag))
                {
                    query = query.Where(a => a.article_tags.Any(at => at.tag.name == tag));
                }

                query = query.OrderBy(a => a.created);

                query = query.Skip((offset - 1) * PageSize).Take(PageSize);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving articles with filters - Keyword: {keyword}, Tag: {tag}, Offset: {offset}: {ex.Message}", ex);
                throw;
            }
        }
    }
}
