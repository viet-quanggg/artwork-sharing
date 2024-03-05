using ArtworkSharing.Core.Helpers.VNPAYS;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.VNPAYS;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ArtworkSharing.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;

        Vnpay Vnpay { get; set; } = new Vnpay();
        public PaymentService(IConfiguration configuration)
        {
            _configuration= configuration;
            _configuration.GetSection("Vnpay").Bind(Vnpay);
        }

        public string GetUrlFromTransaction(OrderInfor orderInfor)
        {
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", Vnpay.TmnCode);
            vnpay.AddRequestData("vnp_Amount", (orderInfor.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Artwork Sharing Payment");
            vnpay.AddRequestData("vnp_OrderType", "other"); 
            vnpay.AddRequestData("vnp_ReturnUrl", Vnpay.ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", orderInfor.OrderId.ToString()); 
            return vnpay.CreateRequestUrl(Vnpay.Url, Vnpay.HashSetcret);
        }
        public async Task<IpnResponseViewModel> ProcessIpnCallback(IpnModel ipnModel, IQueryCollection queries)
        {

            var ipnResponseViewModel = new IpnResponseViewModel();

            if (!ValidateHash(ipnModel, queries))
            {

                ipnResponseViewModel.RspCode = "99";
                ipnResponseViewModel.Message = ResponseMessage.ValidateHashError;

                return ipnResponseViewModel;
            }

            var transaction = await _unitOfWork.TransactionRepository.FirstOrDefaultAsync(t => t.TxnRef == ipnModel.vnp_TxnRef);

            if (transaction != null)
            {

                ipnResponseViewModel.RspCode = "02";
                ipnResponseViewModel.Message = ResponseMessage.TxnRefExist;

                return ipnResponseViewModel;
            }

            var orderGuid = new Guid(ipnModel.vnp_TxnRef);

            var order = await _unitOfWork.Order.FirstOrDefaultAsync(o => o.Id == orderGuid);

            if (order == null)
            {
                ipnResponseViewModel.RspCode = "01";
                ipnResponseViewModel.Message = ResponseMessage.OrderNotFound;

                return ipnResponseViewModel;
            }


            //convert back to normal amount
            ipnModel.vnp_Amount /= 100;

            if (order.Amount != ipnModel.vnp_Amount)
            {

                ipnResponseViewModel.RspCode = "04";
                ipnResponseViewModel.Message = ResponseMessage.AmountNotValid;

                return ipnResponseViewModel;
            }

            // 00 : success, other: fail
            if (ipnModel.vnp_ResponseCode == "00" && ipnModel.vnp_TransactionStatus == "00")
            {
                //********

                // doin sth with db


            }


            ipnResponseViewModel.RspCode = "00";
            ipnResponseViewModel.Message = ResponseMessage.PaymentSuccess;

            return ipnResponseViewModel;
        }
    }
}
