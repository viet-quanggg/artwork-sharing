using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class PaypalOrderRepository : Repository<PaypalOrder>, IPaypalOrderRepository
    {
        public PaypalOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
