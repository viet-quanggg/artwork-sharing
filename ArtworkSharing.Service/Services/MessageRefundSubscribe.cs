using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Helpers.MsgQueues;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ArtworkSharing.Service.Services
{
    public class MessageRefundSubscribe : BackgroundService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScope;
        private readonly IEnumerable<MessageChanel> _messageChanels;
        private readonly IConfiguration _configuration;
        private readonly MessageConnection _getChannel;
        private IModel _channel;

        public MessageRefundSubscribe(IEnumerable<MessageChanel> messageChanels, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _getChannel = new MessageConnection(_configuration);
            _serviceScope = serviceScopeFactory;
            _messageChanels = messageChanels;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MessageChanel messageChanel = new MessageChanel
            {
                ExchangeName = Exchange.RefundPaidRaise,
                QueueName = Queue.RefundPaidRaiseQueue,
                RoutingKey = RoutingKey.PaidRaise
            };
            _channel = _getChannel.InititalBus(messageChanel);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (s, e) =>
            {
                var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                body = body.Replace("\\", "");
                body = body.Trim('"');
                if (!string.IsNullOrEmpty(body))
                {
                    var data = JsonConvert.DeserializeObject<TransactionViewModel>(body);
                    if (data != null)
                    {
                        var type = data.Type;
                        if (type == Core.Domain.Enums.TransactionType.Artwork)
                        {
                            using (var scope = _serviceScope.CreateScope())
                            {
                                switch (data.Type)
                                {
                                    case TransactionType.Artwork:
                                        await UpdateArtwork(data);
                                        break;
                                    case TransactionType.ArtworkService:
                                        await UpdateArtworkService(data);
                                        break;
                                    case TransactionType.Package:
                                        await UpdatePackage(data);
                                        break;
                                }
                            }
                        }
                        _channel.BasicAck(e.DeliveryTag, false);
                    }
                }
            };
            _channel.BasicConsume(messageChanel.QueueName, false, consumer);
            await Task.CompletedTask;
        }

        private async Task UpdateArtwork(TransactionViewModel transactionViewModel)
        {
            await Task.CompletedTask;
        }
        private async Task UpdateArtworkService(TransactionViewModel transactionViewModel)
        {
            await Task.CompletedTask;
        }
        private async Task UpdatePackage(TransactionViewModel transactionViewModel)
        {
            await Task.CompletedTask;
        }
    }
}
