using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Core.ViewModels.Transactions
{
    public class TransactionCreateModel
    {
        public Guid? ArtworkId { get; set; }
        public Guid? AudienceId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public float TotalBill { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }
    }
}
