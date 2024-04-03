using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.ViewModels.MediaContent;

public class MediaContentViewModel
{
    public Guid ArtworkId { get; set; }
    public float Capacity { get; set; }
    public string Media { get; set; } = null!;
    public string MediaWithoutWatermark { get; set; } = null!;
  
}