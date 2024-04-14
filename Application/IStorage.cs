namespace Application
{
    public interface IStorage
    {
        public Task StoreFileAsync(string blobName, Stream content);
    }
}