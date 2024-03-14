using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class VNPayTransactionTransferRepository : Repository<VNPayTransactionTransfer>, IVNPayTransactionTransferRepository
    {
        private readonly DbContext _context;

        public VNPayTransactionTransferRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public void UpdateVNPayTransactionTransfer(VNPayTransactionTransfer vNPay)
        {
            var entry = _context.Entry(vNPay);
            if (entry.State == EntityState.Detached)
            {
                _context.Attach(entry);
            }
            _context.Entry(vNPay).State = EntityState.Modified;
        }
    }
}
