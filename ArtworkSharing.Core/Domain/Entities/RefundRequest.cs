using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class RefundRequest : EntityBase<Guid>
    {
        public Guid TransactionId { get; set; }
        public DateTime RefundRequestDate { get; set; }
        public string Description { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Transaction Transaction { get; set; } = null!;
    }
}
