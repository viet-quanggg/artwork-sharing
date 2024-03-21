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
        /// <summary>
        /// Temp
        /// </summary>
        /// <returns></returns>
        private async Task Update()
        {
            using (var _scope = _serviceScope.CreateScope())
            {
                var _packageService = _scope.ServiceProvider.GetRequiredService<IPackageService>();
                // updating
            }
            await Task.CompletedTask;
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
                        switch (data.Type)
                        {
                            case TransactionType.Artwork: await Update(); break;
                            case TransactionType.ArtworkService: break;
                            case TransactionType.Package: break;
                        }
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            };
            _channel.BasicConsume(messageChanel.QueueName, false, consumer);
            await Task.CompletedTask; // Temp
        }
    }
}
