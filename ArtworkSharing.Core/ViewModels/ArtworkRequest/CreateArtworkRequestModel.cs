using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Core.ViewModels.ArtworkRequest;

public class CreateArtworkRequestModel
{
    public Guid AudienceId { get; set; }
    public Guid ArtistId { get; set; }
    public Guid TransactionId { get; set; }
    public string? Description { get; set; }
    public DateTime RequestedDate { get; set; }
    public float RequestedPrice { get; set; }
    public float RequestedDeposit { get; set; }
    public DateTime RequestedDeadlineDate { get; set; }
    public ArtworkServiceStatus Status { get; set; }
}