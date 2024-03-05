using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class Package : EntityBase<Guid>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public float Price { get; set; }
        public int Duration { get; set; }
    }
}
