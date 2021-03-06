﻿using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Saves an image to the filesystem
        /// </summary>
        /// <param name="imageFile">The imagefile to save</param>
        /// <returns>Filepath</returns>
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            // If the file is null or not an image
            if (imageFile == null)
            {
                throw new ArgumentNullException("imageFile");
            }
            else if (imageFile.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == false)
            {
                throw new ArgumentException("The provided file is not an image.", "imageFile");
            }

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
