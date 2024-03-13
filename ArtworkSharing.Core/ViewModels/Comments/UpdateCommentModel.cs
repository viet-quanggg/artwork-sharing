namespace ArtworkSharing.Core.ViewModels.Comments;

public class UpdateCommentModel
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
}