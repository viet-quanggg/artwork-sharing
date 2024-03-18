using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Core.ViewModels.Transactions;

public class TransactionsViewModelUser
{
    public Guid Id { get; set; }
    public Guid? PackageId { get; set; }
    public Guid? ArtworkId { get; set; }
    public Guid? ArtworkServiceId { get; set; }
    public Guid AudienceId { get; set; }
    public float TotalBill { get; set; }
    public DateTime CreatedDate { get; set; }
    public TransactionStatus Status { get; set; }
    public TransactionType Type { get; set; }

    public Domain.Entities.User Audience { get; set; }
    public Artwork? Artwork { get; set; }
}