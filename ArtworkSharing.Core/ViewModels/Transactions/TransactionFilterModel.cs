using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Core.ViewModels.Transactions;

public class TransactionFilterModel
{
    public Guid? PackageId { get; set; } = Guid.Empty;
    public Guid? ArtworkId { get; set; } = Guid.Empty;
    public Guid? ArtworkServiceId { get; set; } = Guid.Empty;
    public Guid? AudienceId { get; set; } = Guid.Empty;
    public float TotalBillFrom { get; set; } = -1;
    public float TotalBillTo { get; set; } = -1;
    public DateTime? CreatedDateFrom { get; set; } = DateTime.MinValue;
    public DateTime? CreatedDateTo { get; set; } = DateTime.MinValue;
    public TransactionStatus? Status { get; set; } = null!;
    public TransactionType? Type { get; set; } = null!;
    public int PageSize { get; set; } = -1;
    public int PageIndex { get; set; } = -1;
}