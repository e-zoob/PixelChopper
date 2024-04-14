using Infrastructure;
using Infrastructure.Configuration;
using PixelChopper.Application;

var builder = WebApplication.CreateBuilder(args);

var configs = builder.Configuration.GetSection("MyConfig");
builder.Services.Configure<AppConfig>(configs);

builder.Services.AddScoped<IProcessorService, ImageProcessorService>();

var app = builder.Build();

app.MapPost("/api/resize", async (IFormFile file, IProcessorService uploadHandler) =>
{
    try
    {
        await uploadHandler.ProcessImageAsync(file);
        return Results.Ok();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }

    
}).DisableAntiforgery();
app.Run();
