using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IPaymentEventService
    {
        Task<List<PaymentEvent>> GetPaymentEvents();
        Task RemovePaymentEvent(PaymentEvent paymentEvent);
        Task AddPaymentEvent(PaymentEvent paymentEvent);
    }
}
