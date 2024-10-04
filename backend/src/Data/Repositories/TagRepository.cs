using Data.DbContexts;
using Data.Entities;
using Data.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    /// <summary>
    /// Repository for managing tag-related data operations.
    /// </summary>
    public class TagRepository : ITagRepository
    {
        private readonly ArticleHubDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing tag data.</param>
        public TagRepository(ArticleHubDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously retrieves a list of tags by their names.
        /// </summary>
        /// <param name="tagNames">A list of tag names to search for.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of found tags.</returns>
        public async Task<List<Tag>> GetTagsByNamesAsync(List<string> tagNames)
        {
            return await _context.tag
                                 .Where(t => tagNames.Contains(t.name))
                                 .ToListAsync();
        }
    }
}
