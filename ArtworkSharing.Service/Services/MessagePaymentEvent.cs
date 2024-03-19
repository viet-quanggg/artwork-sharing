using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace ArtworkSharing.Service.Services
{
    public class MessagePaymentEvent : BackgroundService, IDisposable
    {
        private readonly IMessageSupport _messageSupport;
        private readonly IServiceScopeFactory _scopeService;
        private CancellationTokenSource _wakeupCancelationTokenSource = new CancellationTokenSource();

        public MessagePaymentEvent(IServiceScopeFactory scopeFactory, IConfiguration configuration, IMessageSupport messageSupport)
        {
            _messageSupport = messageSupport;
            _scopeService = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishPaymentEvents(stoppingToken);
            }
        }
        public void StartPublishingOutstandingIntegrationEvents()
        {
            _wakeupCancelationTokenSource.Cancel();
        }

        private async Task PublishPaymentEvents(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var _scope = _scopeService.CreateScope())
                    {
                        var _paymentEventService = _scope.ServiceProvider.GetRequiredService<IPaymentEventService>();
                        var events = await _paymentEventService.GetPaymentEvents();
                        foreach (var e in events)
                        {
                            await _messageSupport.RaiseEventPayment(new Core.Models.MessageRaw
                            {
                                ExchangeName = Exchange.PaidRaise,
                                Message = e.Data,
                                QueueName = Queue.PaidRaiseQueue,
                                RoutingKey = RoutingKey.PaidRaise
                            });
                            await _paymentEventService.RemovePaymentEvent(e);
                        }
                    }

                    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_wakeupCancelationTokenSource.Token, stoppingToken);
                    try
                    {
                        await Task.Delay(Timeout.Infinite, linkedCts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        if (_wakeupCancelationTokenSource.Token.IsCancellationRequested)
                        {
                            var tmp = _wakeupCancelationTokenSource;
                            _wakeupCancelationTokenSource = new CancellationTokenSource();
                            tmp.Dispose();
                        }
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
            catch
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
