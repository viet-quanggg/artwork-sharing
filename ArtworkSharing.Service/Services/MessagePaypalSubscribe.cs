using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Helpers.MsgQueues;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ArtworkSharing.Service.Services
{
    public class MessagePaypalSubscribe : BackgroundService, IDisposable
    {
        private readonly MessageConnection _msgConnection;
        private IModel _chanel;

        public MessagePaypalSubscribe(MessageConnection messageConnection)
        {
            _msgConnection = messageConnection;
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
            consumer.Received += (sender, args) =>
            {
                var body = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
                body = body.Replace("\\", "");
                body = body.Trim('"');

                if (!string.IsNullOrEmpty(body))
                {
                    var data = JsonConvert.DeserializeObject<TransactionViewModel>(body)!;
                    if (data != null)
                    {
                        if (data.Type == TransactionType.Artwork)
                        {
                            // udpate action
                        }
                        _chanel.BasicAck(args.DeliveryTag, false);
                    }
                }
            };
            _chanel.BasicConsume(messageChanel.QueueName, false, consumer);
            await Task.CompletedTask;
        }
    }
}
