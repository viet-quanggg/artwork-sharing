using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.ViewModels.Artists;

public class ArtistViewModel
{
    public Guid UserId { get; set; }
    public ICollection<ArtworkService>? ArtworkServices { get; set; }
    public ICollection<Artwork>? Artworks { get; set; }
    public ICollection<ArtistPackage>? ArtistPackages { get; set; }
}