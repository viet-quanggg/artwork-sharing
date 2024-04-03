using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IPaymentRefundEventService
    {
        Task CreatePaymentRefundEvent(PaymentRefundEvent refundEvent);
        Task RemovePaymentRefundEvent(PaymentRefundEvent refundEvent);
        Task<List<PaymentRefundEvent>> GetPaymentRefundEvents();
    }
}
