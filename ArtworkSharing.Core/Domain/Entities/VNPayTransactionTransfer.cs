using ArtworkSharing.Core.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class VNPayTransactionTransfer : EntityBase<Guid>
    {
        public Guid TransactionId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
