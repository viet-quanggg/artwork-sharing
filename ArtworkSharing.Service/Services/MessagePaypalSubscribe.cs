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
    public class MessagePaypalSubscribe : BackgroundService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScope;
        private readonly MessageConnection _msgConnection;
        private IModel _chanel;

        public MessagePaypalSubscribe(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScope = serviceScopeFactory;
            _msgConnection = new MessageConnection(configuration);
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MessageChanel messageChanel = new MessageChanel
            {
                ExchangeName = Exchange.PaypalPaidRaise,
                QueueName = Queue.PaypalPaidRaiseQueue,
                RoutingKey = RoutingKey.PaypalPaidRaise
            };
            _chanel = _msgConnection.InititalBus(messageChanel);
            var consumer = new EventingBasicConsumer(_chanel);
            consumer.Received += async (sender, args) =>
            {
                var body = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
                body = body.Replace("\\", "");
                body = body.Trim('"');

                if (!string.IsNullOrEmpty(body))
                {
                    var data = JsonConvert.DeserializeObject<TransactionViewModel>(body)!;
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
                        _chanel.BasicAck(args.DeliveryTag, false);
                    }
                }
            };
            _chanel.BasicConsume(messageChanel.QueueName, false, consumer);
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
            using (var scope = _serviceScope.CreateScope())
            {
                var svc = scope.ServiceProvider.GetRequiredService<IPackageService>();
                await svc.CheckOutPackage(transactionViewModel);
            }
        }
    }
}
