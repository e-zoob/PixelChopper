using Infrastructure.Utilities;
using Microsoft.Net.Http.Headers;

namespace Test;

public class MultipartRequestHelperTests
{

    [Fact]
    public void HasFileContentDisposition_WhenContentDispositionIsFile_ReturnsTrue()
    {
        // Arrange
        var contentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = "image.jpg"
        };

        // Act
        var result = RequestHelper.HasFileContentDisposition(contentDisposition);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasFileContentDisposition_WhenContentDispositionIsNotFile_ReturnsFalse()
    {
        // Arrange
        var contentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "field"
        };

        // Act
        var result = RequestHelper.HasFileContentDisposition(contentDisposition);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImageFile_WhenContentDispositionIsImage_ReturnsTrue()
    {
        // Arrange
        var contentType = "image/jpeg";

        // Act
        var result = RequestHelper.IsImageFile(contentType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsImageFile_WhenContentDispositionIsNotImage_ReturnsFalse()
    {
        // Arrange
        var contentType = "text/plain";

        // Act
        var result = RequestHelper.IsImageFile(contentType);

        // Assert
        Assert.False(result);
    }
}