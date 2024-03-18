using ArtworkSharing.Core.Domain.Base;
using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Core.Domain.Entities;

public class Package : EntityBase<Guid>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public float Price { get; set; }
    public int Duration { get; set; }

    public PackageStatus Status { get; set; }
}