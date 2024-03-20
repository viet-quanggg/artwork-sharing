using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IPaypalPaymentEventService
    {
        Task<List<PaypalPaymentEvent>> GetPaypalPaymentEvents();

        Task RemovePaypalPaymentEvent(PaypalPaymentEvent paypalPaymentEvent);

        Task AddPaypalPaymentEvent(PaypalPaymentEvent paypalPaymentEvent);
    }
}
