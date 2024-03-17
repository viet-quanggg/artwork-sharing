using ArtworkSharing.Core.ViewModels.Transactions;

namespace ArtworkSharing.Core.ViewModels.VNPAYS;

public class VNPayViewModel
{
    public IpnResponseViewModel IpnResponseViewModel { get; set; }
    public TransactionViewModel TransactionViewModel { get; set; }
}