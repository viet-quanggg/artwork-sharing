using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities;

public class Comment : EntityBase<Guid>
{
    public Guid CommentedUserId { get; set; }
    public Guid ArtworkId { get; set; }
    public DateTime CommentedDate { get; set; }
    public string Content { get; set; } = null!;

    public User CommentedUser { get; set; } = null!;
    public Artwork Artwork { get; set; } = null!;
}