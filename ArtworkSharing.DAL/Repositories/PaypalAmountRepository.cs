using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class PaypalAmountRepository : Repository<PaypalAmount>, IPaypalAmountRepository
    {
        public PaypalAmountRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
