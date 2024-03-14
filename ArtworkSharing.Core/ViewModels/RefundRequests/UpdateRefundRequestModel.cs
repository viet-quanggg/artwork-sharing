namespace ArtworkSharing.Core.ViewModels.RefundRequests;

public class UpdateRefundRequestModel
{
    public Guid TransactionId { get; set; }
    public DateTime RefundRequestDate { get; set; }
    public string Description { get; set; } = null!;
    public string Reason { get; set; } = null!;


        public string Status { get; set; } = null!;

   

        // Add whatever you need
    }

