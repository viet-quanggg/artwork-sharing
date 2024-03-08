using ArtworkSharing.Core.ViewModels.Transactions;

namespace ArtworkSharing.Core.ViewModels.VNPAYS
{
    public class VNPayResponseModel
    {
        public IpnResponseViewModel IpnResponseViewModel { get; set; }
        public TransactionViewModel TransactionViewModel { get; set; }
    }
}
