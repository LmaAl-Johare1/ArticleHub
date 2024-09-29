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
        Task<Article> GetArticleByIdAsync(int articleId);

    }
}
