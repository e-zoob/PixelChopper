using System.Text;
using Application;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Common;
using System.Text.Json;

namespace Infrastructure
{
    public class RabbitMQNotificationService : INotifyService
    {
        private readonly IModel _channel;
        public RabbitMQNotificationService(IConfiguration configuration)
        {
            var factory = new ConnectionFactory () { HostName = "rabbitmq" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare(queue: "notification_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public Task SendMessageAsync(BlobMessage blobMessage)
        {
            ArgumentNullException.ThrowIfNull(blobMessage);

            var message = JsonSerializer.Serialize(blobMessage);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "notification_queue",
                                  basicProperties: null,
                                  body: body);

            return Task.CompletedTask;
        }
    }
}