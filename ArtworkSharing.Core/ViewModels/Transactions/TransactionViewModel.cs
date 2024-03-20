using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Core.ViewModels.Transactions;

public class TransactionViewModel
{
    public Guid? PackageId { get; set; }=Guid.Empty;
    public Guid? ArtworkId { get; set; }
    public Guid? ArtworkServiceId { get; set; }
    public Guid AudienceId { get; set; }
    public float TotalBill { get; set; }
    public DateTime CreatedDate { get; set; }
    public TransactionType Type { get; set; }

}