using ArtworkSharing.Core.ViewModels.Categories;
using ArtworkSharing.Core.ViewModels.Comments;
using ArtworkSharing.Core.ViewModels.Likes;
using ArtworkSharing.Core.ViewModels.MediaContent;
using ArtworkSharing.Core.ViewModels.Users;

namespace ArtworkSharing.Core.ViewModels.Artworks;

public class ArtworkViewModel
{
    public Guid ArtistId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public float Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Status { get; set; }

    public UserViewModel Artist { get; set; } = null!;
    public List<MediaContentViewModel> MediaContents { get; set; } = null!;
    public List<LikeViewModel>? Likes { get; set; }
    public List<CommentViewModel>? Comments { get; set; }
    public List<CategoryViewModel>? Categories { get; set; }
}