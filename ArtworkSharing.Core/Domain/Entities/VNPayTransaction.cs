using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class VNPayTransaction : EntityBase<Guid>
    {
        public double Amount { get; set; }
        public string BankCode { get; set; }
        public string BankTranNo { get; set; }
        public string CardType { get; set; }
        public DateTime PayDate { get; set; }
        public string TmnCode { get; set; }
        public string TransactionNo { get; set; }
        public string TxnRef { get; set; }
        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}
