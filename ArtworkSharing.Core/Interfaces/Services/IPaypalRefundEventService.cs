using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IPaypalRefundEventService
    {
        Task<List<PaypalRefundEvent>> GetPaypalRefundEvents();

        Task RemovePaypalRefundEvent(PaypalRefundEvent paypalRefundEvent);

        Task AddPaypalRefundEvent(PaypalRefundEvent paypalRefundEvent);
    }
}
