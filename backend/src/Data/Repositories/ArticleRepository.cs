using Data.DbContexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleHubDbContext _context;
        public ArticleRepository(ArticleHubDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Article article)
        {

            await _context.article.AddAsync(article);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Update(Article article)
        {
            _context.article.Update(article);
        }
        public async Task<Article> GetArticleByIdAsync(int articleId)
        {
            return await _context.article
                .Include(a => a.user)
                .Include(a => a.article_tags)
                .ThenInclude(at => at.tag)
                .FirstOrDefaultAsync(a => a.id == articleId);
        }
        public async Task LikeArticleAsync(int currentUserId, int articleToLikeId)
        {
            var timestamp = DateTime.UtcNow;
            var articleLike =
                new ArticleLike { article_id = articleToLikeId, user_id = currentUserId,created= timestamp,updated=timestamp };
            await _context.article_like.AddAsync(articleLike);
        }
        public void UnLikeArticle(int currentUserId, int articleToUnLikeId)
        {
            var articleLike =
                new ArticleLike { article_id = articleToUnLikeId, user_id = currentUserId };
            _context.article_like.Remove(articleLike);
        }
        public async Task<bool> IsLikedAsync(int UserId, int articleId)
        {
            var isLiked =
               await _context.article_like.AnyAsync(af => af.user_id == UserId && af.article_id == articleId);
            return isLiked;
        }
        public async Task<Article> GetArticleAsync(string slug)
        {
            if (String.IsNullOrEmpty(slug))
            {
                throw new ArgumentNullException(nameof(slug));
            }

            var article = await _context.article.Include(a => a.article_tags)
                                                 .Include(a => a.user)
                                                 .FirstOrDefaultAsync(a => a.slug == slug);
            return article;
        }
    }
}
