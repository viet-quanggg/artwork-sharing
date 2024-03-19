namespace ArtworkSharing.Core.ViewModels.Categories;

public class CategoryViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}