using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    private readonly DbContext _dbContext;

    public TransactionRepository(DbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void UpdateTransaction(Transaction transaction)
    {
        _dbContext.Update(transaction);
    }
}