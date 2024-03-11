namespace ArtworkSharing.Core.ViewModels.Likes
{
    public class LikeViewModel
    {
        public Guid Id { get; set; }
        public Guid LikedUserId { get; set; }
        public Guid ArtworkId { get; set; }
    }
}
