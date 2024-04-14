using Application;
using Infrastructure;
using PixelChopper.Application;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddScoped<IProcessorService, RequestHandlerService>();
builder.Services.AddScoped<IStorage, BlobStorageService>();
builder.Services.AddScoped<INotifyService, RabbitMQNotificationService>();

var app = builder.Build();

app.MapPost("/api/resize", async (IFormFile file, IProcessorService uploadHandler) =>
{
    try
    {
        var blobId = await uploadHandler.ProcessImageAsync(file);
        return Results.Accepted($"/api/image/{blobId}");
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }

    
}).DisableAntiforgery();

app.MapGet("/api/image/{id}", async (string id, IStorage storageService) =>
{
    try
    {
        var imageStream = await storageService.RetrieveFileAsync(id);
        return Results.File(imageStream, "image/jpeg");
    }
    catch (ArgumentException ex)
    {
        return Results.NotFound(ex.Message);
    }
});
app.Run();
