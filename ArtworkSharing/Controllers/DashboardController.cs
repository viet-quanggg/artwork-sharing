using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ITransactionService _TransactionService;
        private readonly ILogger<ManageOrderArtistController> _logger;
        private readonly IArtworkService _ArtworkService;

        public DashboardController(ITransactionService transactionService, ILogger<ManageOrderArtistController> logger, IArtworkService artworkService)
        {
            _TransactionService = transactionService;
            _logger = logger;
            _ArtworkService = artworkService;
        }

        [HttpGet(Name = "Getalltransactionfordashboard")]
        public async Task<ActionResult<ManageOrderArtistController>> GetTransactionsByTimeRange(string timeRange)
        {
            try
            {
                DateTime startDate;

                switch (timeRange.ToLower())
                {
                    case "day":
                        startDate = DateTime.Now.AddDays(-1);
                        break;
                    case "month":
                        startDate = DateTime.Now.AddMonths(-1);
                        break;
                    case "year":
                        startDate = DateTime.Now.AddYears(-1);
                        break;
                    default:
                        return BadRequest("Invalid time range. Supported values are 'day', 'month', and 'year'.");
                }

                var transactions = await _TransactionService.GetAll();
                var filteredTransactions = transactions.Where(t => t.CreatedDate >= startDate);
                return Ok(filteredTransactions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting transactions: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet(Name = "GetallArtworkforDashboard")]
        public async Task<ActionResult<ManageOrderArtistController>> GetArtWork()
        {
            try
            {
                var transactions = await _ArtworkService.GetAll();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting transactions: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{entityId}", Name = "GetWorkerforDashboard")]
        public async Task<ActionResult<ManageOrderArtistController>> GetWorker(Guid entityId)
        {
            try
            {
                var worker = await _ArtworkService.GetOne(entityId);
                return Ok(worker);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting transactions: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
