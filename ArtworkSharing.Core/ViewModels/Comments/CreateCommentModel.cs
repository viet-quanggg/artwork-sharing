namespace ArtworkSharing.Core.ViewModels.Comments;

public class CreateCommentModel
{
    public Guid ArtworkId { get; set; }
    public string Content { get; set; } = null!;
}