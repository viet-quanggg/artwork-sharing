using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Extensions;
using ArtworkSharing.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ArtworkSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IArtworkService _artworkService;
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService, IArtworkService artworkService, IPaymentService paymentService)
    {
        _paymentService = paymentService;
        _artworkService = artworkService;
        _transactionService = transactionService;
    }

    /// <summary>
    ///     Get transaction by filter
    /// </summary>
    /// <param name="transactionFilterModel"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromBody] TransactionFilterModel transactionFilterModel)
    {
        return Ok(await _transactionService.GetTransactions(transactionFilterModel));
    }

    /// <summary>
    ///     Get transaction by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransaction([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest();

        return Ok(await _transactionService.GetTransaction(id));
    }

    [Authorize]
    [HttpGet("userTransactions")]
    public async Task<ActionResult> GetUserTransaction()
    {
        var id = HttpContext.Items["UserId"];
        if (id == null) return Unauthorized();

        Guid uid = Guid.Parse(id + "");

        if (uid == Guid.Empty) return Unauthorized();

        return Ok(await _transactionService.GetTransactionsForUser(uid));
    }

    [Authorize]
    [HttpPost("/CreateTransactionForArtworkServiceDeposit")]
    public async Task<IActionResult> CreateTransactionForArtworkServiceDeposit(Guid artworkServiceId, Guid paymentMethodId)
    {
        var id = HttpContext.Items["UserId"];
        if (id == null) return Unauthorized();

        Guid uid = Guid.Parse(id + "");

        if (uid == Guid.Empty) return Unauthorized();
        if (uid == null) return BadRequest();
        return Ok(await _transactionService.CreateTransactionForArtworkRequestDeposit(artworkServiceId, uid, paymentMethodId));
    }

    [HttpGet("/Count")]
    public async Task<IActionResult> GetalltransactionforChart(string timeRange)
    {
        if (timeRange == null) return BadRequest();
        DateTime startDate;
        if (timeRange.IsNullOrEmpty())
        {
            startDate = DateTime.Now.AddYears(-1);
        }
        else
        {
            switch (timeRange.ToLower())
            {
                case "day":
                    startDate = DateTime.Now.AddDays(-10);
                    break;
                case "month":
                    startDate = DateTime.Now.AddMonths(-5);
                    break;
                case "year":
                    startDate = DateTime.Now.AddYears(-5);
                    break;
                default:
                    return BadRequest("Invalid time range. Supported values are 'day', 'month', and 'year'.");
            }
        }
        var transactions = await _transactionService.GetAudience();
        var filteredTransactions = transactions.Where(t => t.CreatedDate >= startDate);

        return Ok(filteredTransactions);
    }


    [ArtworkSharing.Extensions.Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTransactionArtwork(TransactionArtworkModel transactionArtworkModel)
    {
        //var idRaw = HttpContext.Items["UserId"];
        //if (idRaw == null) return Unauthorized();

        //Guid uid = Guid.Parse(idRaw + "");

        //if (uid == Guid.Empty) return Unauthorized();

        var uid = Guid.Parse("48485956-80A9-42AB-F8C2-08DC44567C01");
        var artwork = await _artworkService.GetArtwork(transactionArtworkModel.ArtworkId);

        if (artwork == null) return BadRequest(new { Message = "Not found artwork" });

        TransactionCreateModel transactionCreateModel = new TransactionCreateModel
        {
            ArtworkId = artwork.Id,
            AudienceId = uid,
            PaymentMethodId = transactionArtworkModel.PaymentMethodId,
            Status = Core.Domain.Enums.TransactionStatus.Success,
            TotalBill = artwork.Price,
            Type = Core.Domain.Enums.TransactionType.Artwork
        };

        var rs = await _transactionService.CreateTransactionArtwork(transactionCreateModel);

        if (rs == null) return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok(_paymentService.GetUrlFromTransaction(rs));
    }
}
