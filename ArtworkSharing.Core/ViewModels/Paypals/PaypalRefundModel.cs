namespace ArtworkSharing.Core.ViewModels.Paypals
{
    public class PaypalRefundModel
    {
        public string note_to_payer { get; set; } 
        public RefundAmount amount { get; set; }
    }
    public class RefundAmount
    {
        public string currency_code { get; set; }
        public double value { get; set; }

    }
}
