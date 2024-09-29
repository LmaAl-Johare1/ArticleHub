using Data.DbContexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
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
        public async Task<Article> GetArticleByIdAsync(int articleId)
        {
            return await _context.article
                .Include(a => a.user)
                .Include(a => a.article_tags)
                .ThenInclude(at => at.tag)
                .FirstOrDefaultAsync(a => a.id == articleId);
        }
    }
}
