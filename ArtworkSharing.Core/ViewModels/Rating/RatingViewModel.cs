using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.ViewModels.Rating;

public class RatingViewModel
{
    public float Star { get; set; }
    public string? Content { get; set; }
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;
}