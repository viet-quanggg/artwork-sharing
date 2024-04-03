using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IPaymentMethodService
    {
        Task<List<PaymentMethod>> GetPaymentMethods();

        Task<PaymentMethod> GetPaymentMethod(Guid id);
    }
}
