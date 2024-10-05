using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IArticleRepository
    {
        Task AddAsync(Article article);
        Task<int> SaveChangesAsync();
        void Update(Article article);
        Task<Article> GetArticleByIdAsync(int articleId);
        Task LikeArticleAsync(int currentUserId, int articleToLikeId);
        void UnLikeArticle(int currentUserId, int articleToUnLikeId);
        Task<bool> IsLikedAsync(int UserId, int articleId);
        Task<Article> GetArticleAsync(string slug);
    }
}
