using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class PaypalAmount:EntityBase<Guid>
    {
        public string CurrencyCode { get; set; }
        public double Value { get; set; }
        public string ItemTotalCurrencyCode { get; set; }
        public double ItemTotalValue { get; set; }
        public Guid  PaypalOrderId { get; set; }
        public PaypalOrder PaypalOrder { get; set; }
    }
}

