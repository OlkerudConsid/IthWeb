using IthWeb.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.IO;
using Xunit;

namespace IthWeb.Tests
{
    public class ImageFileServiceTests
    {
        private readonly IImageFileService _imageFileService;

        public ImageFileServiceTests()
        {
            _imageFileService = new ImageFileService();
        }

        [Fact]
        public async void SaveImage_ShouldNotAllowNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _imageFileService.SaveImage(null));
        }

        [Fact]
        public async void SaveImage_ShouldNotAllowNonImageFiles()
        {
            // Using the moq nuget package for creating mock data
            var fileMock = new Mock<IFormFile>();

            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var contentType = "application/pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);

            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            
            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns(fileName);
            fileMock.Setup(x => x.Length).Returns(ms.Length);
            fileMock.Setup(x => x.ContentType).Returns(contentType);

            var file = fileMock.Object;
            
            await Assert.ThrowsAsync<ArgumentException>(async () => await _imageFileService.SaveImage(file));
        }
    }
}
