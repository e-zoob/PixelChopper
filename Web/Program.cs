using Infrastructure;
using PixelChopper.Application;

var builder = WebApplication.CreateBuilder(args);

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
