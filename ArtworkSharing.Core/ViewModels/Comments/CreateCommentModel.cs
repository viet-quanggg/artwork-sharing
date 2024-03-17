namespace ArtworkSharing.Core.ViewModels.Comments;

public class CreateCommentModel
{
    public Guid CommentedUserId { get; set; }
    public Guid ArtworkId { get; set; }
    public string Content { get; set; } = null!;
}