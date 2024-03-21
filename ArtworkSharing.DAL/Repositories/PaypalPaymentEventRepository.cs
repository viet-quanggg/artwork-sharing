using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class PaypalPaymentEventRepository : Repository<PaypalPaymentEvent>, IPaypalPaymentEventRepository
    {
        public PaypalPaymentEventRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
