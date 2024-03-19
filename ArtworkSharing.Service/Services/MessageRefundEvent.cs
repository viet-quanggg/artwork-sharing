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

        public MessageRefundEvent(IServiceScopeFactory serviceScope,IMessageSupport messageSupport)
        {
            _messageSupport= messageSupport;
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
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScope.CreateScope())
                {
                    var _paymentRefundService = scope.ServiceProvider.GetRequiredService<IPaymentRefundEventService>();
                    var events = await _paymentRefundService.GetPaymentRefundEvents();
                    foreach (var item in events)
                    {
                        _messageSupport.RaiseEventPayment(new Core.Models.MessageRaw { 
                        ExchangeName=Exchange.RefundPaidRaise
                        });
                        await _paymentRefundService.RemovePaymentRefundEvent(item);
                    }
                }
            }
        }
    }
}
