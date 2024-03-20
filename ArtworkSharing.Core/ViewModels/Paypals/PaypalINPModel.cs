using ArtworkSharing.Core.ViewModels.Transactions;

namespace ArtworkSharing.Core.ViewModels.Paypals
{
    public class PaypalINPModel
    {
        public int Code { get; set; }
        public TransactionViewModel TransactionViewModel { get; set; }
    }
}
