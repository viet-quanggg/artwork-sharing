using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[Route("[controller]")]
[ApiController]
public class ManageOrderController : Controller
{
    private readonly ITransactionService _transactionService;

    public ManageOrderController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionViewModel>>> GetAllTransactions()
    {
        var transactions = await _transactionService.GetAll();
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionViewModel>> GetTransaction(Guid id)
    {
        var transaction = await _transactionService.GetTransaction(id);
        if (transaction == null) return NotFound();
        return Ok(transaction);
    }

    //[HttpPost]
    //public async Task<ActionResult<TransactionViewModel>> CreateTransaction(CreateTransactionModel ctm)
    //{
    //	// Validate ctm if needed
    //	var createdTransaction = await _transactionService.CreateTransaction(ctm);
    //	return CreatedAtAction(nameof(GetTransaction), new { id = createdTransaction.Id }, createdTransaction);
    //}

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(Guid id, UpdateTransactionModel utm)
    {
        try
        {
            var updatedTransaction = await _transactionService.UpdateTransaction(id, utm);
            return Ok(updatedTransaction);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        var result = await _transactionService.DeleteTransaction(id);
        if (result) return NoContent();
        return NotFound();
    }
}