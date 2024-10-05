using Core.Models;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IArticleService
    {
        Task<ArticleDto> CreateArticleAsync(ArticleForCreationDto articleForCreationDto, string username);
        Task<ArticleDto> EditArticleAsync(int articleId, ArticleForUpdateDto articleForUpdateDto, string username);
        Task<ArticleDto> GetArticleByIdAsync(int articleId);
        Task<bool> LikeArticleAsync(string slug);
        Task<bool> UnLikeArticleAsync(string slug);
    }
}
