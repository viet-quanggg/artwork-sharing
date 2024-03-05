namespace ArtworkSharing.Core.ViewModels.Likes
{
    public class CreateLikeModel
    {
        public Guid LikedUserId { get; set; }
        public Guid ArtworkId { get; set; }
    }
}
