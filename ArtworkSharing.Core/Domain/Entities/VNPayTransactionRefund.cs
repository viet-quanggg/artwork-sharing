using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class VNPayTransactionRefund : EntityBase<Guid>
    {
        public string ResponseId { get; set; }
        public string TmnCode { get; set; }
        public string TxnRef { get; set; }
        public double Amount { get; set; }
        public string BankCode { get; set; }
        public DateTime PayDate { get; set; }
        public string TransactionNo { get; set; }
        public Guid TransactionId { get; set; }
    }
}
