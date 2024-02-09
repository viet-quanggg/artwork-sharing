using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class RefundRequestRepository : Repository<RefundRequest>, IRefundRequestRepository
    {
        public RefundRequestRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
