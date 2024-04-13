using Microsoft.AspNetCore.Http;

namespace PixelChopper.Application;

public interface IUploadHandler
{
    public Task HandleUploadAsync(HttpContext context);
}
