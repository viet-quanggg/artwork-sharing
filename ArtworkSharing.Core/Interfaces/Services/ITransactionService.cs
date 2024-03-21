using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Transactions;
using System.Linq.Expressions;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface ITransactionService
{
        Task<TransactionViewModel> CreateTransactionForArtworkRequestDeposit(Guid artworkrequestId, Guid audienceId, Guid paymentMethodId);
        Task<List<TransactionViewModel>> GetAll();
        Task<TransactionViewModel> GetTransaction(Guid id);
        Task<Transaction> GetOne(Guid id);
        Task<TransactionViewModel> UpdateTransaction(Guid transactionId, UpdateTransactionModel utm);
        Task<bool> DeleteTransaction(Guid id);
        Task<List<TransactionViewModel>> GetTransactions(TransactionFilterModel transactionFilter);

        Task<List<TransactionsViewModelUser>> GetTransactionsForUser(Guid userId);

        Task<TransactionViewModel> AddTransaction(Transaction transaction);

    IEnumerable<Transaction> Get(
Expression<Func<Transaction, bool>> filter = null,
Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy = null,
string includeProperties = "",
int? pageIndex = null,
int? pageSize = null
);

    Task<int> Count(Expression<Func<Transaction, bool>> filter = null);

}
