namespace ArtworkSharing.Core.ViewModels.Artworks;

public class UpdateArtworkModel
{
    public Guid Id { get; set; }
    public Guid ArtistId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public float Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Status { get; set; }

    public List<Guid> MediaContentIds { get; set; } = null!;
    public List<Guid>? CategoryIds { get; set; }
}