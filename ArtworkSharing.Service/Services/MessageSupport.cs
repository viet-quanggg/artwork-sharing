using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using IModel = RabbitMQ.Client.IModel;

namespace ArtworkSharing.Service.Services
{
    public class MessageSupport : IMessageSupport, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceScopeFactory _scopeService;
        private readonly IConfiguration _configuration;

        public MessageSupport(IConfiguration configuration, IServiceScopeFactory serviceScope)
        {
            _scopeService = serviceScope;
            _configuration = configuration;
            InitialBus();
        }
        private void InitialBus()
        {
            try
            {
                ConnectionFactory _factory = new ConnectionFactory();
                _factory.HostName = _configuration["RabbitMQHost"];
                _factory.Port = Convert.ToInt32(_configuration["RabbitMQPort"]);
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at InitialBus: " + ex.Message);
            }
        }

        private void InitialBroker(MessageRaw raw)
        {
            if (!_connection.IsOpen)
            {
                InitialBus();
            }
            _channel.ExchangeDeclare(raw.ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(raw.QueueName, false, false, false, null!);
            _channel.QueueBind(raw.QueueName, raw.ExchangeName, raw.RoutingKey, null!);
        }

        private void SendRequest(MessageRaw raw)
        {
            InitialBroker(raw);
            var rawSerial = JsonConvert.SerializeObject(raw.Message);
            var body = Encoding.UTF8.GetBytes(rawSerial);
            _channel.ConfirmSelect();
            _channel.BasicPublish(raw.ExchangeName, raw.RoutingKey, null!, body);
            _channel.WaitForConfirms((new TimeSpan(0, 0, 5)));
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public async Task RaiseEventPayment(MessageRaw messageRaw, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                SendRequest(messageRaw);
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at PaymentRaise: " + ex.Message);
            }
            finally
            {
                Dispose();
            }
        }
    }
}
