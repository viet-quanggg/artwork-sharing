using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Helpers.MsgQueues;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text.Json;


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
            MessageChanel messageChanel = new MessageChanel
            {
                ExchangeName = Exchange.PaidRaise,
                QueueName = Queue.PaidRaiseQueue,
                RoutingKey = RoutingKey.PaidRaise
            };
            _channel = _getChannel.InititalBus(messageChanel);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) =>
            {
                var body = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                body = body.Replace("\\", "");
                var newBody = body.Trim('"');

                if (!string.IsNullOrEmpty(newBody))
                {
                    var data = System.Text.Json.JsonSerializer.Deserialize<TransactionViewModel>(newBody);
                    if (data != null)
                    {
                        var type = data.Type;
                        switch (type)
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
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            };
            _channel.BasicConsume(messageChanel.QueueName, false, consumer);
            await Task.CompletedTask; // Temp
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
