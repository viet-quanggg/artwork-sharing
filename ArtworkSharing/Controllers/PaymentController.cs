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
    private readonly IPaymentEventService _paymentEventService;
    private readonly MessagePaymentEvent _messagePaymentEvent;
    private readonly ITransactionService _transactionService;
    private readonly IVNPayTransactionService _VNPayTransactionService;
    private readonly IMessageSupport _messageSupport;

    public PaymentController(IVNPayTransactionService vNPayTransactionService, ITransactionService transactionService, MessagePaymentEvent messagePaymentEvent, IPaymentEventService paymentEventService)
    {
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
    [HttpGet("{id}")]
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
        return Ok(rs.TransactionViewModel);
    }

    [HttpGet("test")]
    public async Task<IActionResult> TestTest()
    {
        await _paymentEventService.AddPaymentEvent(new Core.Domain.Entities.PaymentEvent { Data = JsonConvert.SerializeObject(new VNPayTransactionTransfer { Id = Guid.NewGuid(), IsCompleted = true, TransactionId = Guid.NewGuid() }) });
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
    [HttpPost("{id}")]
    public async Task<IActionResult> RefundTraction([FromRoute] Guid id)
    {
        //var uId = HttpContext.Items["UserId"] + "";
        //if (string.IsNullOrEmpty(uId)) return Unauthorized();
        var rs = await _VNPayTransactionService.RefundVNPay(id, Guid.Parse("48485956-80A9-42AB-F8C2-08DC44567C01"));
        if (rs.TransactionViewModel == null) return BadRequest(new { rs.IpnResponseViewModel.Message });
        return Ok(rs.TransactionViewModel);
    }
}