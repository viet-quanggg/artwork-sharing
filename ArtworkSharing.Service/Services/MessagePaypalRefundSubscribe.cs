using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Helpers.MsgQueues;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ArtworkSharing.Service.Services
{
    public class MessagePaypalRefundSubscribe : BackgroundService, IDisposable
    {
        private readonly MessageConnection _msgConnection;
        private IModel _chanel;

        public MessagePaypalRefundSubscribe(MessageConnection messageConnection)
        {
            _msgConnection = messageConnection;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MessageChanel messageChanel = new MessageChanel
            {
                ExchangeName = Exchange.RefundPaypaldRaise,
                QueueName = Queue.RefundPaypalRaiseQueue,
                RoutingKey = RoutingKey.RefundPaypalRaise
            };
            _chanel = _msgConnection.InititalBus(messageChanel);

            var consumer = new EventingBasicConsumer(_chanel);
            consumer.Received += (sender, e) =>
            {
                var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                body = body.Replace("\\", "");
                body = body.Trim('"');

                if (!string.IsNullOrEmpty(body))
                {
                    var data = JsonConvert.DeserializeObject<TransactionViewModel>(body);
                    if (data != null)
                    {
                        switch (data.Type)
                        {
                            case TransactionType.Artwork: break;
                            case TransactionType.ArtworkService: break;
                            case TransactionType.Package: break;
                        }
                        _chanel.BasicAck(e.DeliveryTag, false);
                    }
                }
            };
            _chanel.BasicConsume(messageChanel.QueueName, false, consumer);
            await Task.CompletedTask; // Temp
        }
    }
}
