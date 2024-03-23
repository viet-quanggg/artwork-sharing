using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.VNPAYS;
using ArtworkSharing.Extensions;
using ArtworkSharing.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ArtworkSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaypalPaymentEventService _paypalPaymentEventService;
    private readonly IPaypalRefundEventService _paypalRefundEventService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly MessageRefundEvent _messageRefundEvent;
    private readonly IPaymentRefundEventService _paymentRefundEventService;
    private readonly IPaypalOrderService _paypalOrderService;
    private readonly IPaymentEventService _paymentEventService;
    private readonly MessagePaymentEvent _messagePaymentEvent;
    private readonly ITransactionService _transactionService;
    private readonly IVNPayTransactionService _VNPayTransactionService;
    private readonly IMessageSupport _messageSupport;

    public PaymentController(IVNPayTransactionService vNPayTransactionService, ITransactionService transactionService,
        MessagePaymentEvent messagePaymentEvent, IPaymentEventService paymentEventService, IPaypalOrderService paypalOrderService,
        IPaymentRefundEventService paymentRefundEventService, MessageRefundEvent messageRefundEvent, IPaymentMethodService paymentMethodService,
        IPaypalRefundEventService paypalRefundEventService, IPaypalPaymentEventService paypalPaymentEventService)
    {
        _paypalPaymentEventService = paypalPaymentEventService;
        _paypalRefundEventService = paypalRefundEventService;
        _paymentMethodService = paymentMethodService;
        _messageRefundEvent = messageRefundEvent;
        _paymentRefundEventService = paymentRefundEventService;
        _paypalOrderService = paypalOrderService;
        _paymentEventService = paymentEventService;
        _messagePaymentEvent = messagePaymentEvent;
        _transactionService = transactionService;
        _VNPayTransactionService = vNPayTransactionService;
    }

    /// <summary>
    ///     Get url VNPAY redirect by transactionId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("vnpay/{id}")]
    public async Task<IActionResult> GetUrlRedirectVNPAY(Guid id)
    {
        if (id == Guid.Empty) return BadRequest();
        var tran = await _transactionService.GetOne(id);
        if (tran == null!) return BadRequest();

        return Ok(await _VNPayTransactionService.GetUrlFromTransaction(tran));
    }

    /// <summary>
    ///     Return url and process ipnVNPAY
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> ProcessIpnVNPAY()
    {
        var rs = await _VNPayTransactionService.HandleQuery(Request.QueryString + "");
        if (rs.TransactionViewModel == null) return BadRequest(new { rs.IpnResponseViewModel.Message });

        await _paymentEventService.AddPaymentEvent(
         new Core.Domain.Entities.PaymentEvent
         {
             Data = JsonConvert.SerializeObject(rs.TransactionViewModel)
         });
        _messagePaymentEvent.StartPublishingOutstandingIntegrationEvents();

        return Ok(rs.TransactionViewModel);
    }

    [HttpGet("test")]
    public async Task<IActionResult> TestTest()
    {
        await _paymentEventService.AddPaymentEvent(
            new Core.Domain.Entities.PaymentEvent
            {
                Data = JsonConvert.SerializeObject(new VNPayTransactionTransfer { Id = Guid.NewGuid(), IsCompleted = true, TransactionId = Guid.NewGuid() })
            });
        _messagePaymentEvent.StartPublishingOutstandingIntegrationEvents();
        return Ok();
    }

    /// <summary>
    ///     Get VNPay transaction by transactionId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("VNPAYTransaction/{id}")]
    public async Task<IActionResult> GetVNPAYTransaction(Guid id)
    {
        if (id == Guid.Empty) return BadRequest();

        return Ok(await _VNPayTransactionService.GetVNPayTransactionByTransactionId(id));
    }

    /// <summary>
    ///     Get VNPAY Transactions
    /// </summary>
    /// <param name="vNPayFilter"></param>
    /// <returns></returns>
    [HttpGet("VNPAYTransactions")]
    public async Task<IActionResult> GetVNPAYTransactions([FromBody] VNPayFilter vNPayFilter)
    {
        return Ok(await _VNPayTransactionService.GetVNPayTransactions(vNPayFilter));
    }

    /// <summary>
    ///     Refund transaction by transaction id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("{id}")]
    public async Task<IActionResult> RefundTraction([FromRoute] Guid id)
    {
        var idRaw = HttpContext.Items["UserId"];
        if (idRaw == null) return Unauthorized();

        Guid uid = Guid.Parse(idRaw + "");

        if (uid == Guid.Empty) return Unauthorized();

        var rs = await _VNPayTransactionService.RefundVNPay(id, uid);

        if (rs.TransactionViewModel == null) return BadRequest(new { rs.IpnResponseViewModel.Message });
        await _paymentRefundEventService.CreatePaymentRefundEvent(new PaymentRefundEvent
        {
            Data = JsonConvert.SerializeObject(rs.TransactionViewModel)
        });
        _messageRefundEvent.CancelToken();

        return Ok(rs.TransactionViewModel);
    }


    /// <summary>
    /// Get url paypal via transactionId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("paypal/{id}")]
    public async Task<IActionResult> GetPayPal([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { Message = "Not found transaction" });

        var transaction = await _transactionService.GetOne(id);

        if (transaction == null) return BadRequest(new { Message = "Not found transaction" });

        var rs = await _paypalOrderService.CreateOrder(transaction);

        return Ok(rs.Message);
    }

    /// <summary>
    /// Completed transaction by token return
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("paypal/token/")]
    public async Task<IActionResult> CompletedTransaction()
    {
        var query = Request.QueryString + "";
        string token = (query.Split("&")[0]).Split("=")[1];

        if (string.IsNullOrEmpty(token)) return BadRequest();

        var paypal = await _paypalOrderService.GetPaypalOrder(token);

        if (paypal == null) return BadRequest(new { Message = "Not found transaction" });

        var rs = await _paypalOrderService.CompletedOrder(paypal);

        if (rs == null || rs.TransactionViewModel == null) return StatusCode(StatusCodes.Status500InternalServerError);

        await _paypalPaymentEventService.AddPaypalPaymentEvent(
         new Core.Domain.Entities.PaypalPaymentEvent
         {
             Data = JsonConvert.SerializeObject(rs.TransactionViewModel)
         });
        _messagePaymentEvent.StartPublishingOutstandingIntegrationEvents();

        return Ok(rs.TransactionViewModel);
    }

    /// <summary>
    /// Get payment method by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("paymentMethod/{id}")]
    public async Task<IActionResult> GetPaymentMethod([FromRoute] Guid id) => Ok(await _paymentMethodService.GetPaymentMethod(id));

    /// <summary>
    /// Get all payment method
    /// </summary>
    /// <returns></returns>
    [HttpGet("paymentMethod")]
    public async Task<IActionResult> GetPaymentMethods() => Ok(await _paymentMethodService.GetPaymentMethods());


    /// <summary>
    /// Refund transaction by transactionid
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("paypalRefund")]
    public async Task<IActionResult> RefundPaypal([FromRoute] Guid id)
    {
        var idRaw = HttpContext.Items["UserId"];
        if (idRaw == null) return Unauthorized();

        Guid uid = Guid.Parse(idRaw + "");

        if (uid == Guid.Empty) return Unauthorized();

        if (id == Guid.Empty) return BadRequest(new { Message = "Not found transaction" });

        var transaction = await _transactionService.GetOne(id);

        if (transaction == null) return BadRequest(new { Message = "Not found transaction" });

        if (transaction.AudienceId != uid) return Unauthorized();

        var rs = await _paypalOrderService.RefundPaypal(transaction);

        if (rs == null) return BadRequest();

        if (rs.Code != 00) return BadRequest(new { Message = rs.Message });

        await _paypalRefundEventService.AddPaypalRefundEvent(new PaypalRefundEvent
        {
            Data = JsonConvert.SerializeObject(rs.TransactionViewModel)
        });
        _messageRefundEvent.CancelToken();

        return Ok();
    }
}