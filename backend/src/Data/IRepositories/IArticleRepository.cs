using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.IRepositories
{
    public interface IArticleRepository
    {
        Task AddAsync(Article article);
        Task<int> SaveChangesAsync();
        void Update(Article article);
        void Delete(Article article);

        Task<Article> GetArticleByIdAsync(int articleId);
        Task LikeArticleAsync(int currentUserId, int articleToLikeId);
        void UnLikeArticle(int currentUserId, int articleToUnLikeId);
        Task<bool> IsLikedAsync(int UserId, int articleId);
        Task<Article> GetArticleAsync(string slug);
        Task AddCommentAsync(ArticleComment comment);
        Task<List<Article>> GetArticlesAsync(int offset, string keyword, string tag);

        Task<List<ArticleComment>> GetCommentsByArticleIdAsync(int articleId);

    
}
}
