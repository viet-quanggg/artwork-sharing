using ArtworkSharing.Core.Helpers.VNPAYS;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.VNPAYS;
using ArtworkSharing.DAL.Extensions;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace ArtworkSharing.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uow;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;

        Vnpay Vnpay { get; set; } = new Vnpay();
        public PaymentService(IConfiguration configuration, HttpContext httpContext, IUnitOfWork unitOfWork)
        {
            _uow=unitOfWork;
            _httpContext = httpContext;
            _configuration = configuration;
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
            vnpay.AddRequestData("vnp_CreateDate", orderInfor.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContext));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Artwork Sharing Payment");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", Vnpay.ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", orderInfor.OrderId.ToString());
            return vnpay.CreateRequestUrl(Vnpay.Url, Vnpay.HashSetcret);
        }
        private bool ValidateHash(IpnModel ipnModel, IQueryCollection queries)
        {
            var queryRaw = ToQuery(queries);
            var response = Utils.HmacSHA512(Vnpay.HashSetcret, queryRaw);
            return response.Equals(ipnModel.vnp_SecureHash);
        }

        private string ToQuery(IQueryCollection queries)
        {
            StringBuilder str = new StringBuilder();
            foreach (var item in queries)
            {
                if (item.Key.IsNullOrEmpty() || item.Value.IsNullOrEmpty() || item.Key.StartsWith("vnp_") || item.Key == "vnp_SecureHashType" || item.Key == "vnp_SecureHash") continue;

                str.Append($"{WebUtility.UrlEncode(item.Key)}={item.Key}&");
            }
            str.Remove(str.Length - 1, 1);
            return str.ToString();
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

            var transaction = await _.TransactionRepository.FirstOrDefaultAsync(t => t.TxnRef == ipnModel.vnp_TxnRef);

            if (transaction != null)
            {

                ipnResponseViewModel.RspCode = "02";
                ipnResponseViewModel.Message = ResponseMessage.TxnRefExist;

                return ipnResponseViewModel;
            }

            var transId = new Guid(ipnModel.vnp_TxnRef);

            var trans = await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == transId);

            if (trans == null)
            {
                ipnResponseViewModel.RspCode = "01";
                ipnResponseViewModel.Message = ResponseMessage.OrderNotFound;

                return ipnResponseViewModel;
            }


            ipnModel.vnp_Amount /= 100;

            if (trans.TotalBill != ipnModel.vnp_Amount)
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
