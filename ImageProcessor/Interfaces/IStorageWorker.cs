namespace ImageProcessor.Interfaces;

public interface IStorageWorker
{
    public Task<Stream> RetrieveAsync(string blobName);
    public Task UploadAsync(string blobName, Stream stream);
}