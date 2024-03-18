namespace ArtworkSharing.Core.ViewModels.Paypals
{///
    public class PaypalResonse
    {
        /// <summary>
        /// 00:Success, 06:AmountNotValid, 01:TransactionNotFound,99: Another error
        /// </summary>
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
