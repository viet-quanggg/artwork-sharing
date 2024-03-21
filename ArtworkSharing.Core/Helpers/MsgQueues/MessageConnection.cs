using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace ArtworkSharing.Core.Helpers.MsgQueues
{
    public class MessageConnection
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private RabbitMQ.Client.IModel _channel;
        private MessageChanel _messageChanel = new();
        public MessageConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public RabbitMQ.Client.IModel InititalBus(MessageChanel messageChanel)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = _configuration["RabbitMQHost"];
            factory.Port = Convert.ToInt32(_configuration["RabbitMQPort"]);
            factory.RequestedHeartbeat = TimeSpan.FromSeconds(60);
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(messageChanel.QueueName, false, false, false, null!);
            _channel.ExchangeDeclare(messageChanel.ExchangeName, ExchangeType.Direct);
            _channel.QueueBind(messageChanel.QueueName, messageChanel.ExchangeName, messageChanel.RoutingKey);

            return _channel;
        }
    }
}
