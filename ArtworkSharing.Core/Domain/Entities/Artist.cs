using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities;

public class Artist : EntityBase<Guid>
{ 
    public Guid UserId { get; set; }
    public string BankAccount { get; set; }
    public ICollection<ArtworkService>? ArtworkServices { get; set; }
    public string Bio { get; set; } = String.Empty;
    public ICollection<Artwork>? Artworks { get; set; }
    public ICollection<ArtistPackage>? ArtistPackages { get; set; }
    public User User { get; set; } = null!;
}