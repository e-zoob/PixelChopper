using Application;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class BlobStorageService : IStorage
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            var section = configuration.GetSection("AzureBlobStorage:ConnectionString");
            _blobContainerClient = new BlobContainerClient(section.Value, "pixelchopper");
            _blobContainerClient.CreateIfNotExists();
        }

        public async Task StoreFileAsync(string blobName, Stream content)
        {
            ArgumentNullException.ThrowIfNull(blobName);
            ArgumentNullException.ThrowIfNull(content);

            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content);
        }

    }
}