using Common;
namespace Application;

public interface INotifyService
{
    public Task SendMessageAsync(BlobMessage message);
}