using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class PaymentRefundEventRepository : Repository<PaymentRefundEvent>, IPaymentRefundEventRepository
    {
        public PaymentRefundEventRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
