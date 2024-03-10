using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.VNPAYS;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IVNPayTransactionService
    {
        string GetUrlFromTransaction(Transaction trans);
        Task<VNPayResponseModel> HandleQuery(string query);
        Task<List<VNPayTransactionViewModel>> GetVNPayTransactions(VNPayFilter vNPayFilter);
        Task<VNPayTransactionViewModel> GetVNPayTransactionByTransactionId(Guid id);
        Task<VNPayResponseModel> RefundVNPay(Guid id, Guid userId);
    }
}
