using Data.DbContexts;
using Data.Entities;
using Data.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ArticleHubDbContext _context;

        public TagRepository(ArticleHubDbContext context)
        {
            _context = context;
        }
        public async Task<List<Tag>> GetTagsByNamesAsync(List<string> tagNames)
        {
            return await _context.tag
                                 .Where(t => tagNames.Contains(t.name))
                                 .ToListAsync();
        }
    }
}
