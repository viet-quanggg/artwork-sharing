using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.ViewModels.RefundRequests;

public class RefundRequestViewModelUser
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public DateTime RefundRequestDate { get; set; }
    public string Description { get; set; } = null!;
    public string Reason { get; set; } = null!;
    public string Status { get; set; } = null!;
    
    public Transaction? Transaction { get; set; }
    public Artwork? Artwork { get; set; }
}