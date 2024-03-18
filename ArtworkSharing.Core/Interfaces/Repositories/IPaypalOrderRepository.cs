using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Repositories
{
    public interface IPaypalOrderRepository:IRepository<PaypalOrder>
    {
        void UpdatePaypalOrder(PaypalOrder paypalOrder);
    }
}
