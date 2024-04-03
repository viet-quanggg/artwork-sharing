using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Paypals;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IPaypalOrderService
    {
        Task<string> GetToken();
        Task<PaypalResonse> CreateOrder(Transaction transaction);
        Task<PaypalOrder> GetPaypalOrder(string token);
        Task<PaypalINPModel> CompletedOrder(PaypalOrder paypalOrder);
        Task<PaypalINPModel> RefundPaypal(Transaction tran);
    }
}
