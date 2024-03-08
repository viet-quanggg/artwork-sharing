namespace ArtworkSharing.Core.ViewModels.Comments
{
    public class DeleteCommentModel
    {
        public Guid CommentedUserId { get; set; }
        public Guid ArtworkId { get; set; }
    }
}
