using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IthWeb.Services
{
    public interface IImageFileService
    {
        /// <summary>
        /// Saves the image and returns the path.
        /// </summary>
        /// <param name="imageFile">The file to save.</param>
        /// <returns>Path to saved image</returns>
        public Task<string> SaveImage(IFormFile imageFile);

        /// <summary>
        /// Deletes the specified image if it exists.
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted.</param>
        /// <returns>True upon successful deletion.</returns>
        public Task<bool> DeleteImage(string fileName);
    }
}
