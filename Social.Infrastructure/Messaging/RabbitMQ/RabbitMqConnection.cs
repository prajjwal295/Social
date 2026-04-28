using RabbitMQ.Client;

namespace Social.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMqConnection
    {
        public async Task<IConnection> CreateConnectionAsync()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            return await factory.CreateConnectionAsync();
        }
    }
}
