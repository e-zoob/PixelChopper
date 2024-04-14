using Application;
using Infrastructure;
using PixelChopper.Application;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddScoped<IProcessorService, ImageProcessorService>();
builder.Services.AddScoped<IStorage, BlobStorageService>();
builder.Services.AddScoped<INotifyService, RabbitMQNotificationService>();

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
