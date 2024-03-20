using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Service.Services
{
    public class MessageRefundEvent : BackgroundService, IDisposable
    {
        private readonly IMessageSupport _messageSupport;
        private readonly IServiceScopeFactory _serviceScope;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public MessageRefundEvent(IServiceScopeFactory serviceScope, IMessageSupport messageSupport)
        {
            _messageSupport = messageSupport;
            _serviceScope = serviceScope;
        }

        public void CancelToken()
        {
            _cancellationTokenSource.Cancel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RaiseRefundRequest(stoppingToken);
            }
        }

        private async Task RaiseRefundRequest(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceScope.CreateScope())
                    {
                        var _paymentRefundService = scope.ServiceProvider.GetRequiredService<IPaymentRefundEventService>();
                        var eventVnpays = await _paymentRefundService.GetPaymentRefundEvents();
                        foreach (var item in eventVnpays)
                        {
                            await _messageSupport.RaiseEventPayment(new Core.Models.MessageRaw
                            {
                                ExchangeName = Exchange.RefundPaidRaise,
                                QueueName = Queue.RefundPaidRaiseQueue,
                                RoutingKey = RoutingKey.RefundPaidRaise,
                                Message = item.Data
                            });
                            await _paymentRefundService.RemovePaymentRefundEvent(item);
                        }

                        var _paypalRefundEventService = scope.ServiceProvider.GetRequiredService<IPaypalRefundEventService>();
                        var eventPaypals = await _paypalRefundEventService.GetPaypalRefundEvents();
                        foreach (var item in eventPaypals)
                        {
                            await _messageSupport.RaiseEventPayment(new Core.Models.MessageRaw
                            {
                                ExchangeName = Exchange.RefundPaypaldRaise,
                                QueueName = Queue.RefundPaypalRaiseQueue,
                                RoutingKey = RoutingKey.RefundPaypalRaise,
                                Message = item.Data
                            });
                            await _paypalRefundEventService.RemovePaypalRefundEvent(item);
                        }
                    }
                    var linkToken = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, stoppingToken);
                    try
                    {
                        await Task.Delay(Timeout.Infinite, linkToken.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        if (_cancellationTokenSource.IsCancellationRequested)
                        {
                            var tmp = _cancellationTokenSource;
                            _cancellationTokenSource = new CancellationTokenSource();
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
