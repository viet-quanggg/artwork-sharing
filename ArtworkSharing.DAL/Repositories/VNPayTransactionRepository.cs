using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class VNPayTransactionRepository : Repository<VNPayTransaction>, IVNPayTransactionRepository
{
    public VNPayTransactionRepository(DbContext dbContext) : base(dbContext)
    {
    }
}