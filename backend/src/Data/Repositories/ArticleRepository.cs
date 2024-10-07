using Data.DbContexts;
using Data.Entities;
using Data.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    /// <summary>
    /// Repository for managing article-related database operations.
    /// </summary>
    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleHubDbContext _context;
        private readonly ILogger<ArticleRepository> _logger;

        private const int PageSize = 6;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for articles.</param>
        /// <param name="logger">The logger for logging errors and information.</param>
        public ArticleRepository(ArticleHubDbContext context, ILogger<ArticleRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Asynchronously adds a new article to the database.
        /// </summary>
        /// <param name="article">The article entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(Article article)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));
            await _context.article.AddAsync(article);
        }

        /// <summary>
        /// Asynchronously saves changes made to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing article in the database.
        /// </summary>
        /// <param name="article">The article entity to update.</param>
        public void Update(Article article)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));
            _context.article.Update(article);
        }

        /// <summary>
        /// Asynchronously retrieves an article by its ID.
        /// </summary>
        /// <param name="articleId">The ID of the article to retrieve.</param>
        /// <returns>The article entity, or null if not found.</returns>
        /// <exception cref="Exception">Thrown when there is an error retrieving the article.</exception>
        public async Task<Article> GetArticleByIdAsync(int articleId)
        {
            try
            {
                return await _context.article
                    .Include(a => a.user)
                    .Include(a => a.article_comments)
                    .Include(a => a.article_likes)
                    .Include(a => a.article_tags)
                    .ThenInclude(at => at.tag)
                    .FirstOrDefaultAsync(a => a.id == articleId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving article by ID {articleId}: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes an article from the database.
        /// </summary>
        /// <param name="article">The article entity to delete.</param>
        public void Delete(Article article)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));

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
        /// Asynchronously likes an article.
        /// </summary>
        /// <param name="currentUserId">The ID of the user liking the article.</param>
        /// <param name="articleToLikeId">The ID of the article to like.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LikeArticleAsync(int currentUserId, int articleToLikeId)
        {
            var timestamp = DateTime.UtcNow;
            var articleLike = new ArticleLike
            {
                article_id = articleToLikeId,
                user_id = currentUserId,
                created = timestamp,
                updated = timestamp
            };

            await _context.article_like.AddAsync(articleLike);
        }

        /// <summary>
        /// Unlikes an article.
        /// </summary>
        /// <param name="currentUserId">The ID of the user unliking the article.</param>
        /// <param name="articleToUnLikeId">The ID of the article to unlike.</param>
        public void UnLikeArticle(int currentUserId, int articleToUnLikeId)
        {
            var articleLike = new ArticleLike { article_id = articleToUnLikeId, user_id = currentUserId };
            _context.article_like.Remove(articleLike);
        }

        /// <summary>
        /// Asynchronously checks if a user has liked a specific article.
        /// </summary>
        /// <param name="UserId">The ID of the user.</param>
        /// <param name="articleId">The ID of the article.</param>
        /// <returns>True if the user has liked the article, otherwise false.</returns>
        public async Task<bool> IsLikedAsync(int UserId, int articleId)
        {
            return await _context.article_like.AnyAsync(af => af.user_id == UserId && af.article_id == articleId);
        }

        /// <summary>
        /// Asynchronously retrieves a list of articles with optional filtering by keyword and tag.
        /// </summary>
        /// <param name="offset">The page number for pagination.</param>
        /// <param name="keyword">The keyword to filter articles by title or body.</param>
        /// <param name="tag">The tag to filter articles by.</param>
        /// <returns>A list of articles matching the filters.</returns>
        /// <exception cref="Exception">Thrown when there is an error retrieving articles.</exception>
        public async Task<List<Article>> GetArticlesAsync(int offset, string keyword, string tag)
        {
            try
            {
                var query = _context.article
                    
                    .AsQueryable();

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(a => a.title.Contains(keyword) || a.body.Contains(keyword));
                }

                if (!string.IsNullOrEmpty(tag))
                {
                    query = query.Where(a => a.article_tags.Any(at => at.tag.name == tag));
                }

               var query1 = await query.Include(a => a.user)
                    .Include(a => a.article_tags)
                    .Include(a => a.article_likes).OrderBy(a => a.created)
                             .Skip((offset - 1) * PageSize)
                             .Take(PageSize).ToListAsync();

                return  query1;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving articles with filters - Keyword: {keyword}, Tag: {tag}, Offset: {offset}: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Asynchronously adds a comment to an article.
        /// </summary>
        /// <param name="comment">The comment entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddCommentAsync(ArticleComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            await _context.article_comment.AddAsync(comment);
        }

        /// <summary>
        /// Asynchronously retrieves comments for a specific article by its ID.
        /// </summary>
        /// <param name="articleId">The ID of the article whose comments to retrieve.</param>
        /// <returns>A list of comments associated with the article.</returns>
        public async Task<List<ArticleComment>> GetCommentsByArticleIdAsync(int articleId)
        {
            return await _context.article_comment
                .Where(c => c.article_id == articleId)
                .Include(c => c.user)
                .ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves an article by its slug.
        /// </summary>
        /// <param name="slug">The slug of the article to retrieve.</param>
        /// <returns>The article entity, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the slug is null or empty.</exception>
        public async Task<Article> GetArticleAsync(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                throw new ArgumentNullException(nameof(slug));
            }

            return await _context.article
                .Include(a => a.article_tags)
                .Include(a => a.user)
                .FirstOrDefaultAsync(a => a.slug == slug);
        }
    }
}
