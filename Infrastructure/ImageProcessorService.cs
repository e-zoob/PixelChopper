using Microsoft.AspNetCore.Http;
using Infrastructure.Utilities;
using PixelChopper.Application;
using Microsoft.Net.Http.Headers;

namespace Infrastructure;

public class ImageProcessorService : IProcessorService
{
    private const long MaxFileSize = 10L * 1024L * 1024L; // 10MB
    public async Task ProcessImageAsync(IFormFile file)
    {
        ArgumentNullException.ThrowIfNull(file);

        if (!(file.Length <= MaxFileSize))
        {
            throw new ArgumentException("File size exceeds the limit.");
        }

        if (!RequestHelper.HasFileContentDisposition(ContentDispositionHeaderValue.Parse(file.ContentDisposition)))
        {
            throw new ArgumentException("Invalid content type. Only form data is allowed.");
        }

        if (!RequestHelper.IsImageMimeType(file.ContentType) || !RequestHelper.IsImageFileExtension(file.FileName))
        {
            throw new ArgumentException("Invalid file type. Only image files are allowed.");
        }

        using var targetStream = new MemoryStream();
        await file.CopyToAsync(targetStream);
    }

}