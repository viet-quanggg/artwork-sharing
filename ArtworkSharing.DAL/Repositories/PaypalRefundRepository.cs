using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class PaypalRefundRepository : Repository<PaypalRefund>, IPaypalRefundRepository
    {
        public PaypalRefundRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
