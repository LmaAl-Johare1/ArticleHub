using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IArticleService
    {
        Task<ArticleDto> CreateArticleAsync(ArticleForCreationDto articleForCreationDto, string username);
        Task<ArticleDto> EditArticleAsync(int articleId, ArticleForUpdateDto articleForUpdateDto, string username);
        Task<ArticleDto> GetArticleByIdAsync(int articleId);
        Task<bool> LikeArticleAsync(string slug);
        Task<bool> UnLikeArticleAsync(string slug);
        Task<List<ArticleCardDto>> GetArticlesAsync(ArticlesSearchDto articlesSearch);
        Task<bool> DeleteArticleAsync(int articleId, string username);
        Task<int>GetTotalArticlesCount(string keyword,string tag);
        Task<ArticleCommentDto> AddCommentToArticleAsync(int articleId, ArticleCommentDto commentDto, string username);

    }
}
