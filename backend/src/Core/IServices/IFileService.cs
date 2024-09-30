using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}
