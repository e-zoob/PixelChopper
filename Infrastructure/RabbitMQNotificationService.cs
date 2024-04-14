using System.Text;
using Application;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

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

        public Task SendMessageAsync(string message)
        {
            ArgumentNullException.ThrowIfNull(message);

             var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "notification_queue",
                                  basicProperties: null,
                                  body: body);

            return Task.CompletedTask;
        }
    }
}