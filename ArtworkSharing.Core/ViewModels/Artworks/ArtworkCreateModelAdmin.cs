using System.Text.Json.Serialization;
using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.ViewModels.Artworks;

public class ArtworkCreateModelAdmin
{
    public string? Name { get; set; } 
    public string? Description { get; set; }
    public float Price { get; set; }

    [JsonIgnore]
    public ICollection<MediaContent>? MediaContents { get; set; } = null; 
    [JsonIgnore]
    public ICollection<Category>? Categories { get; set; }
}