using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IthWeb.Services
{
    public class ImageFileService : IImageFileService
    {
        public Task<bool> DeleteImage(string fileName)
        {
            string fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/");
            string fullPath = fileDirectory + fileName;

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public async Task<string> SaveImage(IFormFile imageFile)
        {
            // Get the filename of the uploaded file and the path where we want to save the image.
            var fileExtension = Path.GetExtension(imageFile.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Copy the uploaded file to our target path
                await imageFile.CopyToAsync(fileStream);
            }

            return "https://localhost:44389/images/" + fileName;
        }
    }
}
