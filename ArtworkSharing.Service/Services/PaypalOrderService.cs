using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Paypals;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
            client.BaseAddress = new Uri("api get token");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            var content = new FormUrlEncodedContent(values);

            var authenticationString = $"{_paypalKey.ClientId}:{_paypalKey.ClientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api get token");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = content;

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject(responseBody);

            return "continue";
        }

        public async Task<PaypalResonse> CreateOrder(Transaction transaction)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api-m.paypal.com");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var token = await GetToken();

            if (string.IsNullOrEmpty(token))
            {
                return new PaypalResonse
                {
                    Message = "Access token invalid"
                };
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            PaypalOrderModel paypalOrder = new PaypalOrderModel();
            paypalOrder.intent = "CAPTURE";

            UnitAmount unitAmount = new UnitAmount
            {
                currency_code = "USD",
                value = transaction.TotalBill
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
                value = transaction.TotalBill
            };
            Amount amount = new Amount
            {
                breakdown = new Breakdown
                {
                    item_total = itemTotal
                },
                value = transaction.TotalBill,
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

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject(responseBody);

            return new PaypalResonse
            {
                Message = "Ok man"
            };
        }
    }
}
