using System.Security.Claims;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Extensions;
using ArtworkSharing.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ArtworkSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
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
        if (uid == null) return BadRequest();
        if (uid == Guid.Empty) return BadRequest(new { Message = "User not found!" });
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

}
