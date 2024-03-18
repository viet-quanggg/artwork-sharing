using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class PaypalOrder:EntityBase<Guid>
    {
        public string Intent { get; set; }
        ICollection<PaypalItem> PaypalItems { get; set; }
        ICollection<PaypalAmount> PaypalAmounts { get; set; }
    }
}
