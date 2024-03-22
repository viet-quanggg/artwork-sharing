using ArtworkSharing.Core.ViewModels.Transactions;

namespace ArtworkSharing.Core.ViewModels.Paypals
{
    public class PaypalINPModel
    {
        /// <summary>
        /// 00:Success, 06:AmountNotValid, 01:TransactionNotFound,99: Another error
        /// </summary>
        public string Message { get; set; }
        public int Code { get; set; }
        public TransactionViewModel TransactionViewModel { get; set; }
    }
}
