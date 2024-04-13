using Infrastructure.Utilities;
using Microsoft.Net.Http.Headers;

namespace Test;

public class MultipartRequestHelperTests
{
    [Fact]
    public void GetBoundary_WhenCalled_ReturnsBoundary()
    {
        //Arrange
        var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/form-data");
        mediaTypeHeaderValue.Parameters.Add(new NameValueHeaderValue("boundary", "----WebKitFormBoundarymx2fSWqWSd0OxQqq"));

        var lengthLimit = 100;

        //Act
        var result = MultipartRequestHelper.GetBoundary(mediaTypeHeaderValue, lengthLimit);

        //Assert
        Assert.Equal("----WebKitFormBoundarymx2fSWqWSd0OxQqq", result);
    }

    [Fact]
    public void GetBoundary_WhenBoundaryIsMissing_ThrowsInvalidDataException()
    {
        //Arrange
        var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/form-data");
        var lengthLimit = 100;

        //Act and Assert
        Assert.Throws<InvalidDataException>(() => MultipartRequestHelper.GetBoundary(mediaTypeHeaderValue, lengthLimit));
    }
    
    [Fact]
    public void GetBoundary_WhenBoundaryLengthExceedsLimit_ThrowsInvalidDataException()
    {
        //Arrange
        var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/form-data");
        mediaTypeHeaderValue.Parameters.Add(new NameValueHeaderValue("boundary", "----WebKitFormBoundarymx2fSWqWllll"));
        var lengthLimit = 10;

        //Act and Assert
        Assert.Throws<InvalidDataException>(() => MultipartRequestHelper.GetBoundary(mediaTypeHeaderValue, lengthLimit));
    }

    [Fact]
    public void IsMultipartContentType_WhenContentTypeIsMultipart_ReturnsTrue()
    {
        // Arrange
        var contentType = "multipart/form-data";

        // Act
        var result = MultipartRequestHelper.IsMultipartContentType(contentType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsMultipartContentType_WhenContentTypeIsNotMultipart_ReturnsFalse()
    {
        // Arrange
        var contentType = "application/json";

        // Act
        var result = MultipartRequestHelper.IsMultipartContentType(contentType);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasFileContentDisposition_WhenContentDispositionIsFile_ReturnsTrue()
    {
        // Arrange
        var contentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = "text.txt"
        };

        // Act
        var result = MultipartRequestHelper.HasFileContentDisposition(contentDisposition);

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
        var result = MultipartRequestHelper.HasFileContentDisposition(contentDisposition);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImageFile_WhenContentDispositionIsImage_ReturnsTrue()
    {
        // Arrange
        var contentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = "test.jpg"
        };

        // Act
        var result = MultipartRequestHelper.IsImageFile(contentDisposition);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsImageFile_WhenContentDispositionIsNotImage_ReturnsFalse()
    {
        // Arrange
        var contentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = "test.txt"
        };

        // Act
        var result = MultipartRequestHelper.IsImageFile(contentDisposition);

        // Assert
        Assert.False(result);
    }
}