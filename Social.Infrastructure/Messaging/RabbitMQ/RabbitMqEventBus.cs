using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;
using Social.Application.Common.Interface;

namespace Social.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly RabbitMqConnection _connection;

        public RabbitMqEventBus(RabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task PublishAsync<T>(T @event)
        {
            var conn = await _connection.CreateConnectionAsync(); 
            using var channel = await conn.CreateChannelAsync();

            var queueName = typeof(T).Name;

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: properties,
                body: body
            );
        }
    }
}
