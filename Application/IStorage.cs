namespace Application
{
    public interface IStorage
    {
        public Task StoreFileAsync(string blobName, Stream content);
        public Task<Stream> RetrieveFileAsync(string blobName);
    }
}