using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.IRepositories
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetTagsByNamesAsync(List<string> tagNames);
    }
}
