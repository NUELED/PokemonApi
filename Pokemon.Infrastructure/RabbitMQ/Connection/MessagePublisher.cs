using Pokemon.Application.Common.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokemon.Infrastructure.RabbitMQ.Connection
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        public MessagePublisher(IRabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }   
        public void PublishMessageAsync<T>(string? queueName, T message)
        {
            using var channel = _rabbitMqConnection.Connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);   
        }


    }

}
