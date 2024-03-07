using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;

namespace ArtworkSharing.Service.Services
{
    public class VNPayTransactionService : IVNPayTransactionService
    {
        private readonly IUnitOfWork _uow;

        public VNPayTransactionService(IUnitOfWork unitOfWork)
        {
            _uow= unitOfWork;
        }
        //public Task<TransactionViewModel> CreateVNPayTransaction(VNPayTransaction vNPayTransaction)
        //{
        //}
    }
}
