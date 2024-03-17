using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities;

public class Like : EntityBase<Guid>
{
    public Guid UserId { get; set; }
    public Guid ArtworkId { get; set; }
    public DateTime LikedDate { get; set; }
    public User? LikedUser { get; set; }
    public Artwork? Artwork { get; set; }
}