using System.Security.Claims;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpGet("userTransactions")]
    public async Task<ActionResult> GetUserTransaction()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid currentUserId = new Guid(userIdClaim?.Value);
        if (currentUserId == Guid.Empty) return BadRequest(new { Message = "User not found!" });
        return Ok(await _transactionService.GetTransactionsForUser(currentUserId));
    }

    [HttpPost("/CreateTransactionForArtworkServiceDeposit")]
    public async Task<IActionResult> CreateTransactionForArtworkServiceDeposit(Guid artworkServiceId, Guid paymentMethodId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid currentUserId = new Guid(userIdClaim?.Value);
        if (artworkServiceId == null || currentUserId == null) return BadRequest();
        return Ok(await _transactionService.CreateTransactionForArtworkRequestDeposit(artworkServiceId, currentUserId, paymentMethodId));
    }
    

    
}
