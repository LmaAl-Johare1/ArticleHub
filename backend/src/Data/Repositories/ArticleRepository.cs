using Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleHubDbContext _context;
        public ArticleRepository(ArticleHubDbContext context)
        {
            _context = context;
        }
    }
}
