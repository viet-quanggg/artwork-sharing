using ArtworkSharing.Core.ViewModels.MediaContent;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ArtworkSharing.Core.ViewModels.Artworks;

public class CreateArtworkModel
{
    public Guid ArtistId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [Range(1,10000000000)]
    public float Price { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool Status { get; set; } = true;

    public List<IFormFile> MediaContents { get; set; } = null!;
    public List<Guid>? CategoryIds { get; set; }
}