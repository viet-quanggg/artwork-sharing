using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class PaypalRefundEvent : EntityBase<int>
    {
        public string Data { get; set; }
    }
}
