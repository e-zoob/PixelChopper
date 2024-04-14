namespace Application;

public interface INotifyService
{
    public Task SendMessageAsync(string message);
}