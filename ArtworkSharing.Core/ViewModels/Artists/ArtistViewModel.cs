using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Users;

namespace ArtworkSharing.Core.ViewModels.Artists;

public class ArtistViewModel
{
    public Guid UserId { get; set; }
    public UserViewModel User { get; set; }

    //public ICollection<ArtworkService>? ArtworkServices { get; set; }
    //public ICollection<Artwork>? Artworks { get; set; }
    //public ICollection<ArtistPackage>? ArtistPackages { get; set; }
}