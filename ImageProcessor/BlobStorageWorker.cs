using ImageProcessor.Interfaces;
using Azure.Storage.Blobs;

namespace ImageProcessor;

public class BlobStorageWorker: IStorageWorker
{
    private readonly BlobContainerClient _blobContainerClient;
    public BlobStorageWorker(IConfiguration configuration)
    {
        var section = configuration.GetSection("AzureBlobStorage:ConnectionString");
        _blobContainerClient = new BlobContainerClient(section.Value, "pixelchopper");
    }

    public async Task<Stream> RetrieveAsync(string blobName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        using var stream = new MemoryStream();
        var response = await blobClient.DownloadToAsync(stream);
        stream.Position = 0;
        return stream;
    }

    public async Task UploadAsync(string blobName, Stream stream)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(stream);
    }
  
}