using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Paypals;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Core.ViewModels.VNPAYS;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace ArtworkSharing.Service.Services
{
    public class PaypalOrderService : IPaypalOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private PaypalKey _paypalKey = new PaypalKey();
        private ApplicationContext _applicationContext = new ApplicationContext();
        private double _exchangeCurrency;
        public PaypalOrderService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _exchangeCurrency = double.Parse(_configuration.GetSection("ExchangeCurrency").Value + "");
            _uow = unitOfWork;
            _configuration.GetSection("PaypalKey").Bind(_paypalKey);
            _configuration.GetSection("ApplicationContext").Bind(_applicationContext);
        }

        public async Task<string> GetToken()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            var content = new FormUrlEncodedContent(values);

            var authenticationString = $"{_paypalKey.ClientId}:{_paypalKey.ClientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = content;

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject(responseBody);

            return "continue";
        }
        public async Task<PaypalOrder> GetPaypalOrder(string token) => await _uow.PaypalOrderRepository.FirstOrDefaultAsync(x => x.Token == token);

        public async Task<PaypalINPModel> RefundPaypal(Transaction tran)
        {
            var transactionTransfers =
                await _uow.VNPayTransactionTransferRepository.Where(x => x.TransactionId == tran.Id && x.IsCompleted).ToListAsync();

            if (transactionTransfers.Count == 0) return new PaypalINPModel
            {
                TransactionViewModel = null!,
                Code = 01
            };

            foreach (var item in transactionTransfers)
            {
                var paypalOrder = await _uow.PaypalOrderRepository.Where(x => x.Id == item.Id).FirstOrDefaultAsync();

                using var client = new HttpClient();
                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var authenticationString = $"{_paypalKey.ClientId}:{_paypalKey.ClientSecret}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/v2/payments/captures/{paypalOrder.CaptureId}/refund");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                requestMessage.Headers.Add("Prefer", "return=representation");
                requestMessage.Content = new StringContent("{}");
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var requestMsg = await client.SendAsync(requestMessage);
                string responseBody = await requestMsg.Content.ReadAsStringAsync();

                requestMsg.EnsureSuccessStatusCode();

                JObject jObj = JObject.Parse(responseBody);

                PaypalRefund paypalRefund = new PaypalRefund
                {
                    CurrencyCode = "USD",
                    ExchangeCurrency = _exchangeCurrency,
                    GrossAmount = double.Parse(jObj["seller_payable_breakdown"]!["gross_amount"]!["value"] + ""),
                    NetAmount = double.Parse(jObj["seller_payable_breakdown"]!["net_amount"]!["value"] + ""),
                    PaypalFee = double.Parse(jObj["seller_payable_breakdown"]!["paypal_fee"]!["value"] + ""),
                    TotalRefund = double.Parse(jObj["seller_payable_breakdown"]!["total_refunded_amount"]!["value"] + ""),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Id = paypalOrder.Id
                };
                await _uow.PaypalRefundRepository.AddAsync(paypalRefund);
                await _uow.SaveChangesAsync();
            }
            return new PaypalINPModel
            {
                Code = 00,
                TransactionViewModel = AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(tran)
            };
        }

        /// <summary>
        /// Check invoice on paypal
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<bool> GetInvoice(string token)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authenticationString = $"{_paypalKey.ClientId}:{_paypalKey.ClientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/v2/checkout/orders/{token}");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);


            var response = await client.SendAsync(requestMessage);
            string responseBody = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            JObject jObj = JObject.Parse(responseBody);

            if (jObj["id"] + "" == token && jObj["status"] + "" == "APPROVED") return true;

            return false;
        }

        public async Task<PaypalINPModel> CompletedOrder(PaypalOrder paypalOrder)
        {
            if (await GetInvoice(paypalOrder.Token) is false) return new PaypalINPModel
            {
                TransactionViewModel = null!,
                Code = 99,
                Message = ResponseMessage.TransactionNotPayYet
            };

            var paypal = await _uow.PaypalOrderRepository.FirstOrDefaultAsync(x => x.Id == paypalOrder.Id);

            if (paypal == null) return new PaypalINPModel
            {
                TransactionViewModel = null!,
                Code = 01,
                Message= ResponseMessage.TransactionNotFound
            };
            var transactionTransfer = await _uow.VNPayTransactionTransferRepository.FirstOrDefaultAsync(x => x.Id == paypalOrder.Id && !x.IsCompleted);

            if (transactionTransfer == null) return new PaypalINPModel
            {
                TransactionViewModel = null!,
                Code = 01,
                Message = ResponseMessage.TransactionNotFound

            };

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authenticationString = $"{_paypalKey.ClientId}:{_paypalKey.ClientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/v2/checkout/orders/{paypal.Token}/capture");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Headers.Add("Prefer", "return=representation");
            requestMessage.Content = new StringContent("{}");
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(requestMessage);
            string responseBody = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            JObject jObj = JObject.Parse(responseBody);
            if (jObj["id"] + "" != paypal.Token) return new PaypalINPModel
            {
                TransactionViewModel = null!,
                Code = 01
            };
            var captureId = "";

            // Because project business is 1 transaction 1 quantity 1 item, don't worry i will maintain in the future
            foreach (var item in jObj["purchase_units"]!)
            {
                foreach (var item1 in item["payments"]!["captures"]!)
                {
                    captureId = item1["id"] + "";
                    break;
                }
            }

            paypal.CaptureId = captureId;
            transactionTransfer.IsCompleted = true;
            paypal.Status = "Completed";
            paypal.ModifiedOn = DateTime.Now;

            _uow.VNPayTransactionTransferRepository.UpdateVNPayTransactionTransfer(transactionTransfer);
            _uow.PaypalOrderRepository.UpdatePaypalOrder(paypal);

            await _uow.SaveChangesAsync();

            return new PaypalINPModel
            {
                Code = 00,
                TransactionViewModel = AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == paypalOrder.TransactionId)),
                Message = ResponseMessage.Success
            };
        }

        public double ConvertToDollar(double amount)
        {
            double result = Math.Round(amount / _exchangeCurrency, 2);
            return result;
        }

        public async Task<PaypalResonse> CreateOrder(Transaction transaction)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authenticationString = $"{_paypalKey.ClientId}:{_paypalKey.ClientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            PaypalOrderModel paypalOrder = new PaypalOrderModel();
            paypalOrder.intent = "CAPTURE";

            UnitAmount unitAmount = new UnitAmount
            {
                currency_code = "USD",
                value = ConvertToDollar(transaction.TotalBill)
            };


            ItemTotal itemTotal = new ItemTotal
            {
                currency_code = "USD",
                value = ConvertToDollar(transaction.TotalBill)
            };
            Amount amount = new Amount
            {
                breakdown = new Breakdown
                {
                    item_total = itemTotal
                },
                value = ConvertToDollar(transaction.TotalBill),
                currency_code = "USD"
            };
            string description = "";
            string name = "";

            switch (transaction.Type)
            {
                case Core.Domain.Enums.TransactionType.Artwork:
                    description = transaction.Artwork!.Description!;
                    name = transaction.Artwork.Name; break;
                case Core.Domain.Enums.TransactionType.Package:
                    description = transaction.Package!.Description!;
                    name = transaction.Package.Name; break;
                case Core.Domain.Enums.TransactionType.ArtworkService:
                    description = transaction.ArtworkService!.Description!;
                    name = "ArtworkService"; break;
            }
            List<PurchaseUnit> purchase_units = new List<PurchaseUnit>();
            List<Item> items = new List<Item>();
            items.Add(new Item
            {
                name = name,
                description = description,
                quantity = 1,
                unit_amount = unitAmount
            });
            purchase_units.Add(new PurchaseUnit
            {
                amount = amount,
                items = items
            });
            paypalOrder.purchase_units = purchase_units;
            paypalOrder.application_context = _applicationContext;
            var json = JsonConvert.SerializeObject(paypalOrder);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/v2/checkout/orders");
            requestMessage.Content = new StringContent(json);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Headers.Add("Prefer", "return=representation");
            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.Created)
            {

                var responseBody = await response.Content.ReadAsStringAsync();

                JObject jObj = JObject.Parse(responseBody);

                int a = 0;
                if ((await _uow.PaypalOrderRepository.Where(x => x.Token == jObj["id"] + "").AsQueryable().FirstOrDefaultAsync()) != null)
                {

                    foreach (var item in jObj["links"]!)
                    {
                        if (a == 1)
                        {
                            return new PaypalResonse
                            {
                                Status = 00,
                                Message = item["href"] + ""
                            };
                        }
                        a++;
                    }
                }

                // Because project business is 1 transaction 1 quantity 1 item, don't worry i will maintain in the future

                string merchantId = "", payeeEmail = "";
                int i = 0;
                foreach (var item in jObj["purchase_units"])
                {
                    i++;
                    merchantId = item["payee"]!["merchant_id"] + "";
                    if (i == 1) break;
                }

                PaypalOrder paypal = new PaypalOrder
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    Intent = paypalOrder.intent,
                    MerchantId = merchantId,
                    PayeeEmailAddress = payeeEmail,
                    Status = "Created",
                    Token = jObj["id"] + "",
                    TransactionId = transaction.Id,
                    ExchangeCurrency = _exchangeCurrency
                };

                PaypalItem paypalItem = new PaypalItem
                {
                    CurrencyCode = "VND",
                    Description = description,
                    Id = Guid.NewGuid(),
                    Name = name,
                    PaypalOrderId = paypal.Id,
                    Quantity = 1,
                    Value = ConvertToDollar(transaction.TotalBill),
                };

                PaypalAmount paypalAmount = new PaypalAmount
                {
                    CurrencyCode = "VND",
                    Id = Guid.NewGuid(),
                    ItemTotalCurrencyCode = "VND",
                    ItemTotalValue = ConvertToDollar(transaction.TotalBill),
                    PaypalOrderId = paypal.Id,
                    Value = ConvertToDollar(transaction.TotalBill)
                };
                VNPayTransactionTransfer vNPayTransaction = new VNPayTransactionTransfer
                {
                    Id = paypal.Id,
                    IsCompleted = false,
                    TransactionId = transaction.Id
                };
                await _uow.PaypalOrderRepository.AddAsync(paypal);
                await _uow.PaypalItemRepository.AddAsync(paypalItem);
                await _uow.PaypalAmountRepository.AddAsync(paypalAmount);
                await _uow.VNPayTransactionTransferRepository.AddAsync(vNPayTransaction);

                await _uow.SaveChangesAsync();

                a = 0;
                foreach (var item in jObj["links"]!)
                {
                    if (a == 1)
                    {
                        return new PaypalResonse
                        {
                            Status = 00,
                            Message = item["href"] + ""
                        };
                    }
                    a++;
                }
            }
            return new PaypalResonse
            {
                Status = 99,
                Message = ResponseMessage.Error
            };
        }
    }
}
