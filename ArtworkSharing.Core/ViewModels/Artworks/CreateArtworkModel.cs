namespace ArtworkSharing.Core.ViewModels.Artworks
{
    public class CreateArtworkModel
    {
        public Guid ArtistId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public float Price { get; set; }

        public List<Guid> MediaContentIds { get; set; } = null!;
        public List<Guid>? CategoryIds { get; set; }
    }
}
