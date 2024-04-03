namespace ArtworkSharing.Core.ViewModels.RefundRequests;

public class CreateRefundRequestModel
{
    public Guid TransactionId { get; set; }
    public DateTime RefundRequestDate { get; set; }
    public string Description { get; set; } = null!;
    public string Reason { get; set; } = null!;
}