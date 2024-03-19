using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.ViewModels.MediaContent;

namespace ArtworkSharing.Core.ViewModels.ArtworkRequest;

public class ArtworkRequestViewModel
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
    public ICollection<MediaContentViewModel>? ArtworkProduct { get; set; }
}