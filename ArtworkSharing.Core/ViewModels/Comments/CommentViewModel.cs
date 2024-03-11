using ArtworkSharing.Core.ViewModels.Users;

namespace ArtworkSharing.Core.ViewModels.Comments
{
    public class CommentViewModel
    {
        public Guid CommentedUserId { get; set; }
        public Guid ArtworkId { get; set; }
        public DateTime CommentedDate { get; set; }
        public string Content { get; set; } = null!;

        public UserViewModel CommentedUser { get; set; } = null!;
    }
}
