using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Helpers.VNPAYS;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Core.ViewModels.VNPAYS;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.AspNetCore.Http;
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
        private SortedList<string, string> pParams = new SortedList<string, string>();
        private VNPay Vnpay { get; set; } = new VNPay();

        public PaymentService(IConfiguration configuration, IHttpContextAccessor httpContext, IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _httpContext = httpContext.HttpContext!;
            _configuration = configuration;
            _configuration.GetSection("Vnpay").Bind(Vnpay);
        }

        public string GetUrlFromTransaction(Transaction trans)
        {
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", Vnpay.TmnCode);
            vnpay.AddRequestData("vnp_Amount", (trans.TotalBill * 100).ToString());
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            vnpay.AddRequestData("vnp_CreateDate", trans.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", _httpContext.Connection.RemoteIpAddress + "");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Artwork Sharing Payment");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", Vnpay.ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", trans.Id.ToString());

            return vnpay.CreateRequestUrl(Vnpay.Url, Vnpay.HashSetcret);
        }

        private string GetFromQuery(string query)
        {
            StringBuilder str = new StringBuilder();
            SortedList<string, string> pParams = new SortedList<string, string>();
            string[] strSplit = query.Split('&');
            foreach (var item in strSplit)
            {
                if (!string.IsNullOrEmpty(item) && item.StartsWith("_vnp") && (item.Split("=")).Length > 1)
                {
                    pParams.Add(item.Split("=")[0], item.Split("=")[1]);
                }
            }
            foreach (var item in pParams)
            {
                if (item.Key.IsNullOrEmpty() || item.Value.IsNullOrEmpty() || item.Key.StartsWith("vnp_") || item.Key == "vnp_SecureHashType" || item.Key == "vnp_SecureHash") continue;

                str.Append($"{WebUtility.UrlEncode(item.Key)}={item.Key}&");
            }
            str.Remove(str.Length - 1, 1);
            return str.ToString();
        }

        public async Task<VNPayViewModel> HandleQuery(string query)
        {
            var queryString = GetFromQuery(query);
            var response = Utils.HmacSHA512(Vnpay.HashSetcret, queryString);
            if (!response.Equals(pParams["vnp_SecureHash"] + "", StringComparison.InvariantCultureIgnoreCase))
            {
                return new VNPayViewModel
                {
                    TransactionViewModel = null!,
                    IpnResponseViewModel = new IpnResponseViewModel
                    {
                        Message = ResponseMessage.ValidateHashError,
                        RspCode = "99"
                    }
                };
            }

            VNPayTransaction vNPayTransaction = new VNPayTransaction
            {
                TransactionId = Guid.Parse(pParams["vnp_TxnRef"] + ""),
                Amount = double.Parse(pParams["vnp_Amount"] + ""),
                BankCode = pParams["vnp_BankCode"],
                BankTranNo = pParams["vnp_BankTranNo"],
                CardType = pParams["vnp_CardType"],
                PayDate = DateTime.Parse(pParams["vnp_PayDate"] + ""),
                TmnCode = pParams["vnp_TmnCode"],
                TransactionNo = pParams["vnp_TransactionNo"],
                Id = new Guid(pParams["vnp_TxnRef"] + "")
            };

            var transaction = await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == vNPayTransaction.TransactionId);
            if (transaction == null)
            {
                return new VNPayViewModel
                {
                    TransactionViewModel = null!,
                    IpnResponseViewModel = new IpnResponseViewModel
                    {
                        Message = ResponseMessage.TransactionNotFound,
                        RspCode = "01"
                    }
                };
            }

            if (double.TryParse(pParams["vnp_Amount"] + "", out double amount))
            {
                amount /= 100;

                if (transaction.TotalBill != amount)
                {
                    return new VNPayViewModel
                    {
                        TransactionViewModel = null!,
                        IpnResponseViewModel = new IpnResponseViewModel
                        {
                            Message = ResponseMessage.AmountNotValid,
                            RspCode = "06"
                        }
                    };
                }
            }

            await _uow.VNPayTransactionRepository.AddAsync(vNPayTransaction);
            await _uow.SaveChangesAsync();
            return new VNPayViewModel
            {
                TransactionViewModel = AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(transaction),
                IpnResponseViewModel = new IpnResponseViewModel
                {
                    Message = ResponseMessage.Success,
                    RspCode = "00"
                }
            };
        }


    }
}
