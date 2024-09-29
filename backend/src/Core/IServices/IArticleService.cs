using Core.Models;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IArticleService
    {
        Task<ArticleDto> CreateArticleAsync(ArticleForCreationDto articleForCreationDto, string username);

    }
}
