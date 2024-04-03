using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;

namespace ArtworkSharing.Service.Services
{
    public class VNPayTransactionTransferService : IVNPayTransactionTransferService
    {
        private readonly IUnitOfWork _uow;

        public VNPayTransactionTransferService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task CreateVNPayTransactionTransfer(VNPayTransactionTransfer tran)
        {
            await _uow.VNPayTransactionTransferRepository.AddAsync(tran);
            await _uow.SaveChangesAsync();
        }
    }
}
