using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.IRepositories
{
    /// <summary>
    /// Defines the contract for tag-related data operations.
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        /// Asynchronously retrieves a list of tags by their names.
        /// </summary>
        /// <param name="tagNames">A list of tag names to search for.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of found tags.</returns>
        Task<List<Tag>> GetTagsByNamesAsync(List<string> tagNames);
    }
}
