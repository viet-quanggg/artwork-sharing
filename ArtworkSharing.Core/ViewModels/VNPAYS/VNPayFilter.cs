namespace ArtworkSharing.Core.ViewModels.VNPAYS
{
    public class VNPayFilter
    {
        public DateTime? PayDateFrom { get; set; }
        public DateTime? PayDateTo { get; set; }
        public Guid TransactionId { get; set; }
    }
}
