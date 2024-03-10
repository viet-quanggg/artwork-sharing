using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class MediaContent : EntityBase<Guid>
    {
        public Guid ArtworkId { get; set; }
        public float Capacity { get; set; }
        public string Media { get; set; } = null!;

        public Artwork Artwork { get; set; } = null!;
    }
}
