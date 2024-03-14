using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers
{
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
        /// Get transaction by filter
        /// </summary>
        /// <param name="transactionFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromBody] TransactionFilterModel transactionFilterModel)
        {
            return Ok(await _transactionService.GetTransactions(transactionFilterModel));
        }

        /// <summary>
        /// Get transaction by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction([FromRoute] Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            return Ok(await _transactionService.GetTransaction(id));
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="transactionModel">The transaction data</param>
        /// <returns>The created transaction</returns>
        [HttpPost]
        public async Task<IActionResult> CreateTransactionPackage(Guid PackageId, Guid AudienceId, float totalBill)
        {
            try
            {
                var transactionModel = new TransactionViewModel
                {
                    PackageId = PackageId,
                    AudienceId = AudienceId,
                    TotalBill = totalBill,
                    Id = Guid.NewGuid(),
                    ArtworkServiceId = null,
                    CreatedDate = DateTime.UtcNow,
                    ArtworkId = null,
                    Status = Core.Domain.Enums.TransactionStatus.Success,
                    Type = Core.Domain.Enums.TransactionType.Package
                };


                var createdTransaction = await _transactionService.CreateTransaction(transactionModel);
                return Ok(createdTransaction);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); // Internal Server Error
            }
        }

        /// <summary>
        /// Update an existing transaction
        /// </summary>
        /// <param name="id">The ID of the transaction to update</param>
        /// <param name="transactionModel">The updated transaction data</param>
        /// <returns>The updated transaction</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] UpdateTransactionModel transactionModel)
        {
            try
            {
                var updatedTransaction = await _transactionService.UpdateTransaction(id, transactionModel);
                if (updatedTransaction == null)
                {
                    return NotFound(); // Transaction not found
                }
                return Ok(updatedTransaction);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); // Internal Server Error
            }
        }

    }
}
