using Core.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class FileService : IFileService
    {
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            // Define the path where you want to save the file
            var uploadsFolderPath = Path.Combine("wwwroot", "images"); // Adjust the path as needed
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            // Ensure the directory exists
            Directory.CreateDirectory(uploadsFolderPath);

            // Save the file to the specified path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the relative path to the saved file
            return $"/images/{fileName}"; // Adjust the URL as necessary
        }
    }
}
