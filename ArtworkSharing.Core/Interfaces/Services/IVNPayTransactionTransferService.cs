using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IVNPayTransactionTransferService
    {
        Task CreateVNPayTransactionTransfer(VNPayTransactionTransfer tran);
    }
}
