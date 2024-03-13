using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities;

public class Category : EntityBase<Guid>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Artwork>? Artworks { get; set; }
}