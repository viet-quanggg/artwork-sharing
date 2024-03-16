using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Helpers.MsgQueues;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ArtworkSharing.Service.Services
{
    public class MessageSubscribe : BackgroundService, IDisposable
    {
        private readonly MessageConnection _getChannel;
        private IModel _channel;
        private readonly IEnumerable<MessageChanel> _messageChannels;
        private readonly IServiceScopeFactory _serviceScope;

        public MessageSubscribe(IConfiguration configuration, IEnumerable<MessageChanel> messageChanels, IServiceScopeFactory serviceScope)
        {
            _getChannel = new MessageConnection(configuration);
            _messageChannels = messageChanels;
            _serviceScope = serviceScope;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var item in _messageChannels)
            {
                if (item.RoutingKey == null || item.QueueName == null || item.ExchangeName == null)
                {
                    continue;
                }
                _channel = _getChannel.InititalBus(item);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (sender, ea) =>
                {
                    var body = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

                    var data = JsonConvert.DeserializeObject<TransactionViewModel>(body);
                    //  var dataTemp = JsonConvert.DeserializeObject<VNPayTransactionTransfer>(body);
                    //using (var _scope = _serviceScope.CreateScope())
                    //{
                    //    var _refundRequestService = _scope.ServiceProvider.GetRequiredService<IVNPayTransactionTransferService>();

                    //    await _refundRequestService.CreateVNPayTransactionTransfer(dataTemp);
                    //}
                    if (data!=null)
                    {
                        var type = data.Type;
                        if (type == TransactionType.Artwork)
                        {
                            using (var _scope = _serviceScope.CreateScope())
                            {
                                // waiting khoa

                                //  var _refundRequestService = _scope.ServiceProvider.GetRequiredService<IVNPayTransactionTransferService>();
                                //  await _refundRequestService.CreateVNPayTransactionTransfer();
                                // updating
                            }
                        }
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                };
                _channel.BasicConsume(item.QueueName, false, consumer);
            }
            await Task.CompletedTask;
        }
    }
}
