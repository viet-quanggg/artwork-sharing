using ArtworkSharing.Core.ViewModels.Transactions;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionViewModel>> GetAll();
        Task<TransactionViewModel> GetTransaction(Guid id);
        Task<TransactionViewModel> UpdateTransaction(Guid transactionId, UpdateTransactionModel utm);
        Task<bool> DeleteTransaction(Guid id);
    }
}
