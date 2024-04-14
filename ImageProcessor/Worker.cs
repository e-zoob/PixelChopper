using ImageProcessor.Interfaces;
using Azure.Storage.Blobs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Text.Json;

namespace ImageProcessor;

public class Worker : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IStorageWorker _storageWorker;
    public Worker(IStorageWorker storageWorker)
    {
        _storageWorker = storageWorker;
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "notification_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                var blobMessage = JsonSerializer.Deserialize<BlobMessage>(message);

                var imageStream = await _storageWorker.RetrieveAsync(blobMessage.OriginalImageBlob);
                using var output = new MemoryStream();
                ResizeImage(imageStream, output, 100, 100);
                output.Position = 0;
                await _storageWorker.UploadAsync(blobMessage.ResizedImageBlob, output);
            };

            _channel.BasicConsume(queue: "notification_queue", autoAck: true, consumer: consumer);
        }
    }

    private void ResizeImage(Stream input, Stream output, int width, int height)
    {
        using var image = Image.Load(input);
        image.Mutate(x => x.Resize(width, height));
        image.Save(output,  new JpegEncoder());
    }


}
