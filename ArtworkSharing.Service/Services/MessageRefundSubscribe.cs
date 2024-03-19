using ArtworkSharing.Core.Helpers.MsgQueues;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach (var item in _messageChanels)
            {
                if (string.IsNullOrEmpty(item.RoutingKey) || string.IsNullOrEmpty(item.QueueName) || string.IsNullOrEmpty(item.ExchangeName))
                {
                    continue;
                }
                _channel = _getChannel.InititalBus(item);
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (s, e) =>
                {
                    var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());

                    var data = JsonConvert.DeserializeObject<TransactionViewModel>(body);
                    if (data != null)
                    {
                        var type = data.Type;
                        if (type == Core.Domain.Enums.TransactionType.Artwork)
                        {
                            using (var scope = _serviceScope.CreateScope())
                            {
                                var refundRequestService = scope.ServiceProvider.GetRequiredService<RefundRequestService>();
                                //.....
                                // savechanges
                            }
                        }
                        _channel.BasicAck(e.DeliveryTag, false);
                    }
                };
                _channel.BasicConsume(item.QueueName, false, consumer);
            }
            // Delete later
            await Task.CompletedTask;
        }
    }
}
