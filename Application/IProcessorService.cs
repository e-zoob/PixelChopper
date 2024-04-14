using Microsoft.AspNetCore.Http;

namespace PixelChopper.Application;

public interface IProcessorService
{
    public Task<string> ProcessImageAsync(IFormFile file);
}
