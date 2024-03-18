using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Paypals;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Core.ViewModels.VNPAYS;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
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
        public PaypalOrderService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
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

        public async Task<PaypalINPModel> CompletedOrder(PaypalOrder paypalOrder)
        {
            var paypal = await _uow.PaypalOrderRepository.FirstOrDefaultAsync(x => x.Id == paypalOrder.Id);

            if (paypal == null) return new PaypalINPModel
            {
                TransactionViewModel = null!,
                Code = 01
            };
            var transactionTransfer = await _uow.VNPayTransactionTransferRepository.FirstOrDefaultAsync(x => x.Id == paypalOrder.Id && !x.IsCompleted);

            if (transactionTransfer == null) return new PaypalINPModel
            {
                TransactionViewModel = null!,
                Code = 01
            };

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

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/v2/checkout/orders/{paypal.Token}");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = content;
            var response = await client.SendAsync(requestMessage);
            string responseBody = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            JObject jObj = JObject.Parse(responseBody);
            if (jObj["id"] + "" != paypal.Token) return new PaypalINPModel
            {
                TransactionViewModel = null!,
                Code = 01
            };

            transactionTransfer.IsCompleted = true;
            paypal.Status = "Completed";
            paypal.ModifiedOn = DateTime.Parse(jObj["update_time"] + "");

            _uow.VNPayTransactionTransferRepository.UpdateVNPayTransactionTransfer(transactionTransfer);
            _uow.PaypalOrderRepository.UpdatePaypalOrder(paypal);

            await _uow.SaveChangesAsync();

            return new PaypalINPModel
            {
                Code = 00,
                TransactionViewModel = AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == paypalOrder.TransactionId))
            };
        }

        public  double ConvertToDollar(double amount)
        {
            double exchangeRate = 0.8;
            double result = Math.Round(amount * exchangeRate, 2);
            return result;
        }

        public async Task<PaypalResonse> CreateOrder(Transaction transaction)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api-m.paypal.com");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            var content = new FormUrlEncodedContent(values);

            var authenticationString = $"{_paypalKey.ClientId}:{_paypalKey.ClientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            PaypalOrderModel paypalOrder = new PaypalOrderModel();
            paypalOrder.intent = "CAPTURE";

            UnitAmount unitAmount = new UnitAmount
            {
                currency_code = "USD",
                value = ConvertToDollar(transaction.TotalBill)
            };
            Item items = new Item
            {
                name = transaction.Artwork!.Name,
                description = transaction.Artwork!.Description!,
                quantity = 1,
                unit_amount = unitAmount
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
            List<PurchaseUnit> purchase_units = new List<PurchaseUnit>();
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
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.Created)
            {

                var responseBody = await response.Content.ReadAsStringAsync();

                JObject jObj = JObject.Parse(responseBody);

                VNPayTransactionTransfer vNPayTransaction = new VNPayTransactionTransfer
                {
                    Id = Guid.NewGuid(),
                    IsCompleted = false,
                    TransactionId = transaction.Id
                };

                // Because project business is 1 transaction 1 quantity 1 item, I promise in the future i will maintain it

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
                    CreatedOn = DateTime.Parse(jObj["create_time"] + ""),
                    Intent = paypalOrder.intent,
                    MerchantId = merchantId,
                    PayeeEmailAddress = payeeEmail,
                    Status = "Created",
                    Token = jObj["id"] + "",
                    TransactionId = transaction.Id
                };

            
                PaypalItem paypalItem = new PaypalItem
                {
                    CurrencyCode = items.unit_amount.currency_code,
                    Description = transaction.Artwork!.Description!,
                    Id = Guid.NewGuid(),
                    Name = transaction.Artwork.Name,
                    PaypalOrderId = paypal.Id,
                    Quantity = 1,
                    Value = ConvertToDollar(transaction.TotalBill),
                };

                await _uow.VNPayTransactionTransferRepository.AddAsync(vNPayTransaction);
                await _uow.SaveChangesAsync();

                int a = 0;
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
