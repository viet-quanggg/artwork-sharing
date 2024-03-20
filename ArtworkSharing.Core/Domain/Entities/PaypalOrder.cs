using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class PaypalOrder : EntityBase<Guid>
    {
        public string Token { get; set; }
        public string? CaptureId { get; set; }
        public string Intent { get; set; }
        ICollection<PaypalItem> PaypalItems { get; set; }
        ICollection<PaypalAmount> PaypalAmounts { get; set; }
        public string Status { get; set; }
        public string PayeeEmailAddress { get; set; }
        public string MerchantId { get; set; }
        public double ExchangeCurrency { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}
