using System.Text.Json.Serialization;
using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.ViewModels.Artworks;

public class ArtworkUpdateModelAdmin
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public float Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Status { get; set; }

    [JsonIgnore]
    public ICollection<Domain.Entities.MediaContent>? MediaContents { get; set; } = null!;
    [JsonIgnore]
    public ICollection<Category>? Categories { get; set; }

}