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
        /// <param name="context">The database context used for data operations.</param>
        public TagRepository(ArticleHubDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of tags that match the provided tag names.
        /// </summary>
        /// <param name="tagNames">A list of tag names to search for.</param>
        /// <returns>A <see cref="Task{List{Tag}}"/> containing the matching tags.</returns>
        public async Task<List<Tag>> GetTagsByNamesAsync(List<string> tagNames)
        {
            return await _context.tag
                                 .Where(t => tagNames.Contains(t.name))
                                 .ToListAsync();
        }
    }
}
