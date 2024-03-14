using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IVNPayTransactionTransferService
    {
        Task<VNPayTransactionTransfer> CreateVNPayTransactionTransfer(Guid tran);
    }
}
