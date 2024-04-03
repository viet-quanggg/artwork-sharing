using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities;

public class Rating : EntityBase<Guid>
{
    public float Star { get; set; }
    public string? Content { get; set; }
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;
}