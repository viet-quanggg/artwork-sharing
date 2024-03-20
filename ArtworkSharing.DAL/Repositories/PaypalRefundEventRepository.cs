using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class PaypalRefundEventRepository : Repository<PaypalRefundEvent>, IPaypalRefundEventRepository
    {
        public PaypalRefundEventRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
