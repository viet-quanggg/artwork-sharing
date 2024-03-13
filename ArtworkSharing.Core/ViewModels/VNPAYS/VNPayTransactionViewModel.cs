using ArtworkSharing.Core.ViewModels.Transactions;

namespace ArtworkSharing.Core.ViewModels.VNPAYS;

public class VNPayTransactionViewModel
{
    public double Amount { get; set; }
    public string BankCode { get; set; }
    public string BankTranNo { get; set; }
    public string CardType { get; set; }
    public DateTime PayDate { get; set; }
    public string TmnCode { get; set; }
    public string TransactionNo { get; set; }
    public Guid TransactionId { get; set; }
    public TransactionViewModel Transaction { get; set; }
}