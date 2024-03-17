using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    void UpdateTransaction(Transaction transaction);
}