using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class PaypalItemRepository : Repository<PaypalItem>, IPaypalItemRepository
    {
        public PaypalItemRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
