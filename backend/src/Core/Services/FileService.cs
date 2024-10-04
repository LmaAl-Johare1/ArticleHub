using Core.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Services
{
    /// <summary>
    /// Provides methods for handling file operations.
    /// </summary>
    public class FileService : IFileService
    {
        /// <summary>
        /// Asynchronously saves a file to the server.
        /// </summary>
        /// <param name="file">The file to be saved.</param>
        /// <returns>A task that represents the asynchronous operation, containing the relative path of the saved file.</returns>
        /// <exception cref="DirectoryNotFoundException">Thrown when the upload directory cannot be created.</exception>
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            // Define the uploads folder path
            var uploadsFolderPath = Path.Combine("wwwroot", "images");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            // Create the directory if it doesn't exist
            Directory.CreateDirectory(uploadsFolderPath);

            // Save the file to the specified path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the relative path of the saved file
            return $"/images/{fileName}";
        }
    }
}
