using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class PaypalRefund : EntityBase<Guid>
    {
        public double GrossAmount { get; set; }
        public double PaypalFee { get; set; }
        public double PlatformFee { get; set; }
        public double NetAmount { get; set; }
        public double TotalRefund { get; set; }
        public string CurrencyCode { get; set; }
        public double ExchangeCurrency { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
