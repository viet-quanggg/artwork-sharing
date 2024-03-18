using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class PaypalOrderRepository : Repository<PaypalOrder>, IPaypalOrderRepository
    {
        private readonly DbContext _context;

        public PaypalOrderRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public void UpdatePaypalOrder(PaypalOrder paypalOrder)
        {
            var entry = _context.Entry(paypalOrder);
            if (entry.State == EntityState.Detached)
            {
                _context.Attach(entry);
            }
            _context.Entry(paypalOrder).State = EntityState.Modified;
        }
    }
}
