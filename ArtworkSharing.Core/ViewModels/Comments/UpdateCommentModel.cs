namespace ArtworkSharing.Core.ViewModels.Comments
{
    public class UpdateCommentModel
    {
        public Guid Id { get; set; }
        public Guid CommentedUserId { get; set; }
        public Guid ArtworkId { get; set; }
        public string Content { get; set; } = null!;
    }
}
