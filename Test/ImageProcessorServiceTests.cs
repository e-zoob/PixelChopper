using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Test
{
    public class ImageProcessorServiceTests
    {
        private const long MaxFileSize = 10L * 1024L * 1024L; // 10MB

        [Fact]
        public async Task ProcessImageAsync_ValidImage_DoesNotThrowException()
        {
            // Arrange
            var mockFormFile = new Mock<IFormFile>();

            mockFormFile.Setup(f => f.Length).Returns(MaxFileSize - 1);
            mockFormFile.Setup(f => f.ContentType).Returns("image/jpeg");
            mockFormFile.Setup(f => f.ContentDisposition).Returns("form-data; name=\"file\"; filename=\"test.jpg\"");

            var service = new ImageProcessorService();

            // Act
            await service.ProcessImageAsync(mockFormFile.Object);

            // Assert

        }

        [Fact]
        public async Task ProcessImageAsync_InvalidImageSize_ThrowsException()
        {
            // Arrange
            var mockFormFile = new Mock<IFormFile>();

            mockFormFile.Setup(f => f.Length).Returns(MaxFileSize + 1);
            mockFormFile.Setup(f => f.ContentType).Returns("image/jpeg");
            mockFormFile.Setup(f => f.ContentDisposition).Returns("form-data; name=\"file\"; filename=\"test.jpg\"");

            var service = new ImageProcessorService();

            // Act
            async Task Act() => await service.ProcessImageAsync(mockFormFile.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async Task ProcessImageAsync_InvalidContentType_ThrowsException()
        {
            // Arrange
            var mockFormFile = new Mock<IFormFile>();

            mockFormFile.Setup(f => f.Length).Returns(MaxFileSize - 1);
            mockFormFile.Setup(f => f.ContentType).Returns("application/json");
            mockFormFile.Setup(f => f.ContentDisposition).Returns("form-data; name=\"file\"; filename=\"test.jpg\"");

            var service = new ImageProcessorService();

            // Act
            async Task Act() => await service.ProcessImageAsync(mockFormFile.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async Task ProcessImageAsync_InvalidFileExtension_ThrowsException()
        {
            // Arrange
            var mockFormFile = new Mock<IFormFile>();

            mockFormFile.Setup(f => f.Length).Returns(MaxFileSize - 1);
            mockFormFile.Setup(f => f.ContentType).Returns("image/jpeg");
            mockFormFile.Setup(f => f.ContentDisposition).Returns("form-data; name=\"file\"; filename=\"test.txt\"");

            var service = new ImageProcessorService();

            // Act
            async Task Act() => await service.ProcessImageAsync(mockFormFile.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

    }
}