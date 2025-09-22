using Pokemon.Application.Common.Interfaces;
using RabbitMQ.Client;


namespace Pokemon.Infrastructure.RabbitMQ.Connection
{
    public class RabbitMqConnection : IRabbitMqConnection, IDisposable  
    {
        private IConnection _connection;
        public IConnection Connection => _connection!;

        public RabbitMqConnection()
        {
             InitializeConnection();    
        }

        private void InitializeConnection()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "host.docker.internal",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            _connection = connectionFactory.CreateConnection();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }


    }
}
