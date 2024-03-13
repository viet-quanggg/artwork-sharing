namespace ArtworkSharing.Core.ViewModels.Artworks;

public class BrowseArtworkModel
{
    public Guid? ArtistId { get; set; } = Guid.Empty;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsPopular { get; set; }
    public bool IsAscRecent { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}