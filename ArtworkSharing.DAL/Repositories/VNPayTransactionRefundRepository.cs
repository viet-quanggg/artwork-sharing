using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class VNPayTransactionRefundRepository : Repository<VNPayTransactionRefund>, IVNPayTransactionRefundRepository
    {
        public VNPayTransactionRefundRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
