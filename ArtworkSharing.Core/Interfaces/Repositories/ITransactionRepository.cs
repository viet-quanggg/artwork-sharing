using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Transactions;

namespace ArtworkSharing.Core.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        void UpdateTransaction(Transaction transaction);
    }
}
