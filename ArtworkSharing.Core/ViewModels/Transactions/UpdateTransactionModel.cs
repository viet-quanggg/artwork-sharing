using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Core.ViewModels.Transactions;

public class UpdateTransactionModel
{
    public Guid? PackageId { get; set; }
    public Guid? ArtworkId { get; set; }
    public Guid? ArtworkServiceId { get; set; }
    public Guid AudienceId { get; set; }
    public float TotalBill { get; set; }
    public DateTime CreatedDate { get; set; }
    public TransactionStatus Status { get; set; }

    public TransactionType Type { get; set; }

    public Guid Id { get; set; }

    // Add whatever you need
}