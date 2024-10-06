using Core.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Services
{
    /// <summary>
    /// Service class responsible for handling file operations, such as saving files to the server.
    /// </summary>
    public class FileService : IFileService
    {
        /// <summary>
        /// Asynchronously saves the provided file to the server and returns its relative path.
        /// </summary>
        /// <param name="file">The file to be saved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the relative path of the saved file.</returns>
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadsFolderPath = Path.Combine("wwwroot", "images");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            Directory.CreateDirectory(uploadsFolderPath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/images/{fileName}";
        }
    }
}
