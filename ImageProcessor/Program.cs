using ImageProcessor.Interfaces;
using ImageProcessor;

var builder = Host.CreateApplicationBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IStorageWorker, BlobStorageWorker>();

var host = builder.Build();
host.Run();
