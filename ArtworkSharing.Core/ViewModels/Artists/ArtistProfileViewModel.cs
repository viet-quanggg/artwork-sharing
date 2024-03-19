using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.User;

namespace ArtworkSharing.Core.ViewModels.Artists;

public class ArtistProfileViewModel
{
    public Guid Id { get; set; }
    public string BankAccount { get; set; }
    public ICollection<ArtworkService>? ArtworkServices { get; set; }
    public ICollection<Artwork>? Artworks { get; set; }
    public ICollection<ArtistPackage>? ArtistPackages { get; set; }

    public UserViewModel User { get; set; } = null!;
}