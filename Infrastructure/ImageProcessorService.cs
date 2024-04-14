using Microsoft.AspNetCore.Http;
using Infrastructure.Utilities;
using PixelChopper.Application;
using Microsoft.Net.Http.Headers;
using Application;

namespace Infrastructure;

public class ImageProcessorService : IProcessorService
{
    private readonly IStorage _storage;
    private readonly INotifyService _notifyService;

    public ImageProcessorService(IStorage storage, INotifyService notifyService)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
    }
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

        var blobId = Guid.NewGuid().ToString();

        using var stream = file.OpenReadStream();
        await _storage.StoreFileAsync(blobId, stream);
        await _notifyService.SendMessageAsync(blobId);
    }

}