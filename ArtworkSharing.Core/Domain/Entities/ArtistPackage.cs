using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities;

public class ArtistPackage : EntityBase<Guid>
{
    public Guid ArtistId { get; set; }
    public Guid PackageId { get; set; }
    public Guid TransactionId { get; set; }
    public DateTime PurchasedDate { get; set; }

    public Artist Artist { get; set; } = null!;
    public Package Package { get; set; } = null!;
    public Transaction Transaction { get; set; } = null!;
}