using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class RefundRequestRepository : Repository<RefundRequest>, IRefundRequestRepository
    {
        private readonly DbContext _context;

        public RefundRequestRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public void UpdateRefundRequest(RefundRequest refundRequest)
        {
            _context.Update(refundRequest);
        }
    }
}
