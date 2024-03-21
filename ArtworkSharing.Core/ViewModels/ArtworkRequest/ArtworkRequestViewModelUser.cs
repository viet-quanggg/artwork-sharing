using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Core.ViewModels.ArtworkRequest;

public class ArtworkRequestViewModelUser
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public DateTime RequestedDate { get; set; }
    public float RequestedPrice { get; set; }
    public float RequestedDeposit { get; set; }
    public DateTime RequestedDeadlineDate { get; set; }
    public ArtworkServiceStatus Status { get; set; }
    
    public Domain.Entities.User? Audience { get; set; }
    public Artist? Artist { get; set; }
    public Transaction? Transaction { get; set; }
    public Artwork? Artwork { get; set; }
}