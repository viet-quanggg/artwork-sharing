using System.Globalization;
using System.Net;
using System.Text;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Helpers.VNPAYS;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Core.ViewModels.VNPAYS;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ArtworkSharing.Service.Services;

public class VNPayTransactionService : IVNPayTransactionService
{
    private readonly IConfiguration _configuration;
    private readonly HttpContext _httpContext;

    private readonly IUnitOfWork _uow;
    private readonly SortedList<string, string> pParams = new();

    public VNPayTransactionService(IConfiguration configuration, IHttpContextAccessor httpContext,
        IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
        _httpContext = httpContext.HttpContext!;
        _configuration = configuration;
        _configuration.GetSection("Vnpay").Bind(Vnpay);
    }

    private VNPay Vnpay { get; } = new();

    public string GetUrlFromTransaction(Transaction trans)
    {
        var vnpay = new VnPayLibrary();

        vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
        vnpay.AddRequestData("vnp_Command", "pay");
        vnpay.AddRequestData("vnp_TmnCode", Vnpay.TmnCode);
        vnpay.AddRequestData("vnp_Amount", (trans.TotalBill * 100).ToString());
        vnpay.AddRequestData("vnp_BankCode", "VNBANK");
        vnpay.AddRequestData("vnp_CreateDate", trans.CreatedDate.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", "VND");
        vnpay.AddRequestData("vnp_IpAddr", GetIPAddress());
        vnpay.AddRequestData("vnp_Locale", "vn");
        vnpay.AddRequestData("vnp_OrderInfo", "Artwork Sharing Payment");
        vnpay.AddRequestData("vnp_OrderType", "other");
        vnpay.AddRequestData("vnp_ReturnUrl", Vnpay.ReturnUrl);
        vnpay.AddRequestData("vnp_TxnRef", trans.Id.ToString());

        return vnpay.CreateRequestUrl(Vnpay.Url, Vnpay.HashSetcret);
    }

    public async Task<VNPayResponseModel> HandleQuery(string query)
    {
        var queryString = GetFromQuery(query);
        var response = Utils.HmacSHA512(Vnpay.HashSetcret, queryString);
        if (!response.Equals(pParams["vnp_SecureHash"] + "", StringComparison.InvariantCultureIgnoreCase))
            return new VNPayResponseModel
            {
                TransactionViewModel = null!,
                IpnResponseViewModel = new IpnResponseViewModel
                {
                    Message = ResponseMessage.ValidateHashError,
                    RspCode = "99"
                }
            };
        var vNPayTransaction = new VNPayTransaction
        {
            TransactionId = Guid.Parse(pParams["vnp_TxnRef"] + ""),
            Amount = double.Parse(pParams["vnp_Amount"] + ""),
            BankCode = pParams["vnp_BankCode"],
            BankTranNo = pParams["vnp_BankTranNo"],
            CardType = pParams["vnp_CardType"],
            PayDate = DateTime.ParseExact(pParams["vnp_PayDate"] + "", "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
            TmnCode = pParams["vnp_TmnCode"],
            TransactionNo = pParams["vnp_TransactionNo"],
            Id = new Guid(pParams["vnp_TxnRef"] + "")
        };
        var transaction =
            await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == vNPayTransaction.TransactionId);
        if (transaction == null)
            return new VNPayResponseModel
            {
                TransactionViewModel = null!,
                IpnResponseViewModel = new IpnResponseViewModel
                {
                    Message = ResponseMessage.TransactionNotFound,
                    RspCode = "01"
                }
            };

        if (double.TryParse(pParams["vnp_Amount"] + "", out var amount))
        {
            amount /= 100;

            if (transaction.TotalBill != amount)
                return new VNPayResponseModel
                {
                    TransactionViewModel = null!,
                    IpnResponseViewModel = new IpnResponseViewModel
                    {
                        Message = ResponseMessage.AmountNotValid,
                        RspCode = "06"
                    }
                };
        }

        await _uow.VNPayTransactionRepository.AddAsync(vNPayTransaction);
        await _uow.SaveChangesAsync();
        return new VNPayResponseModel
        {
            TransactionViewModel = AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(transaction),
            IpnResponseViewModel = new IpnResponseViewModel
            {
                Message = ResponseMessage.Success,
                RspCode = "00"
            }
        };
    }

    public async Task<List<VNPayTransactionViewModel>> GetVNPayTransactions(VNPayFilter vNPayFilter)
    {
        var vtrans = _uow.VNPayTransactionRepository.GetAll().AsQueryable();
        if (vNPayFilter.PayDateFrom != null!) vtrans = vtrans.Where(x => x.PayDate >= vNPayFilter.PayDateFrom);
        if (vNPayFilter.PayDateTo != null!) vtrans = vtrans.Where(x => x.PayDate <= vNPayFilter.PayDateTo);
        return AutoMapperConfiguration.Mapper.Map<List<VNPayTransactionViewModel>>(await vtrans.ToListAsync());
    }

    public async Task<VNPayResponseModel> RefundVNPay(Guid id, Guid userId)
    {
        var transVNPay = await _uow.VNPayTransactionRepository.Include(x => x.Transaction)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (transVNPay == null)
            return new VNPayResponseModel
            {
                TransactionViewModel = null!,
                IpnResponseViewModel = new IpnResponseViewModel
                {
                    Message = ResponseMessage.TransactionNotFound,
                    RspCode = "01"
                }
            };

        var vNPayRefund = new VNPayRefundRequestModel
        {
            vnp_RequestId = DateTime.Now.Ticks.ToString(),
            vnp_Version = VnPayLibrary.VERSION,
            vnp_Command = "refund",
            vnp_TmnCode = Vnpay.TmnCode,
            vnp_TransactionType = "02",
            vnp_TxnRef = Convert.ToInt64(transVNPay.Transaction.TotalBill) * 100,
            vnp_Amount = transVNPay.Id + "",
            vnp_TransactionNo = transVNPay.TransactionNo,
            vnp_TransactionDate = transVNPay.PayDate.ToString("yyyyMMddHHmmss"),
            vnp_CreateBy = userId + "",
            vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
            vnp_IpAddr = GetIPAddress(),
            vnp_OrderInfo = "Refund transaction with id: " + id,
            vnp_SecureHash = ""
        };

        var data = vNPayRefund.vnp_RequestId + "|" + vNPayRefund.vnp_Version + "|" + vNPayRefund.vnp_Command + "|" +
                   Vnpay.TmnCode + "|" + vNPayRefund.vnp_TransactionType + "|" + vNPayRefund.vnp_TxnRef + "|" +
                   vNPayRefund.vnp_Amount + "|" + vNPayRefund.vnp_TransactionNo + "|" +
                   vNPayRefund.vnp_TransactionDate + "|" + vNPayRefund.vnp_CreateBy + "|" + vNPayRefund.vnp_CreateDate +
                   "|" + vNPayRefund.vnp_IpAddr + "|" + vNPayRefund.vnp_OrderInfo;

        vNPayRefund.vnp_SecureHash = Utils.HmacSHA512(Vnpay.HashSetcret, data);

        var jsonData = JsonConvert.SerializeObject(vNPayRefund);
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(Vnpay.RfApi)!;
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(jsonData);
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        var strData = "";
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            strData = streamReader.ReadToEnd();
        }

        var vNPayRefundResponse = JsonConvert.DeserializeObject<VNPayRefundResponseModel>(strData)!;

        if (vNPayRefundResponse.vnp_ResponseCode == "00")
        {
            var vNPayTransactionRefund = new VNPayTransactionRefund
            {
                Id = transVNPay.Id,
                Amount = vNPayRefundResponse.vnp_Amount,
                BankCode = vNPayRefundResponse.vnp_BankCode,
                PayDate = DateTime.ParseExact(vNPayRefundResponse.vnp_PayDate, "yyyyMMddHHmmss",
                    CultureInfo.InvariantCulture),
                ResponseId = vNPayRefundResponse.vnp_ResponseId,
                TmnCode = vNPayRefundResponse.vnp_TmnCode,
                TransactionId = transVNPay.Id,
                TransactionNo = vNPayRefundResponse.vnp_TransactionNo,
                TxnRef = vNPayRefundResponse.vnp_TxnRef
            };

            await _uow.VNPayTransactionRefundRepository.AddAsync(vNPayTransactionRefund);
            var rs = await _uow.SaveChangesAsync();
            var tran = await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (tran == null)
                return new VNPayResponseModel
                {
                    TransactionViewModel = null!,
                    IpnResponseViewModel = new IpnResponseViewModel
                    {
                        Message = ResponseMessage.TransactionNotFound,
                        RspCode = "01"
                    }
                };
            if (rs > 0)
                return new VNPayResponseModel
                {
                    TransactionViewModel = AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(tran),
                    IpnResponseViewModel = new IpnResponseViewModel
                    {
                        Message = vNPayRefundResponse.vnp_Message,
                        RspCode = vNPayRefundResponse.vnp_ResponseCode
                    }
                };
            return new VNPayResponseModel
            {
                TransactionViewModel = null!,
                IpnResponseViewModel = new IpnResponseViewModel
                {
                    Message = ResponseMessage.Error,
                    RspCode = "99"
                }
            };
        }

        return new VNPayResponseModel
        {
            TransactionViewModel = null!,
            IpnResponseViewModel = new IpnResponseViewModel
            {
                Message = ResponseMessage.TransactionNotFound,
                RspCode = "01"
            }
        };
    }

    public async Task<VNPayTransactionViewModel> GetVNPayTransactionByTransactionId(Guid id)
    {
        return AutoMapperConfiguration.Mapper.Map<VNPayTransactionViewModel>(
            await _uow.VNPayTransactionRepository.FirstOrDefaultAsync(x => x.TransactionId == id));
    }

    private string GetFromQuery(string query)
    {
        var str = new StringBuilder();
        if (pParams.Count > 0) pParams.Clear();
        query = query.Substring(1);
        var strSplit = query.Split('&');
        foreach (var item in strSplit)
            if (!string.IsNullOrEmpty(item) && item.StartsWith("vnp_") && item.Split("=").Length > 1)
                pParams.Add(item.Split("=")[0], item.Split("=")[1]);
        foreach (var item in pParams)
        {
            if (item.Key.IsNullOrEmpty() || item.Value.IsNullOrEmpty() || item.Key == "vnp_SecureHashType" ||
                item.Key == "vnp_SecureHash") continue;

            str.Append($"{WebUtility.UrlEncode(item.Key)}={WebUtility.UrlEncode(item.Value)}&");
        }

        str.Remove(str.Length - 1, 1);
        return str.ToString().Replace("%2B", "+");
    }

    private string GetIPAddress()
    {
        return Dns.GetHostByName(Dns.GetHostName()).AddressList[1] + "";
    }
}