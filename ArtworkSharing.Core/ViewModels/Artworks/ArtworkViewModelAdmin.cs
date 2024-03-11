using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.ViewModels.Artworks;

public class ArtworkViewModelAdmin
{
    public Guid ArtistId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public float Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Status { get; set; }

    public Artist Artist { get; set; } = null!;
    public ICollection<Transaction>? Transactions { get; set; }
    public ICollection<Category>? Categories { get; set; }
    public ICollection<Domain.Entities.MediaContent> MediaContents { get; set; } = null!;

}