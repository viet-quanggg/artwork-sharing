using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[ApiController]
[Route("[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IArtistService _ArtistService;
    private readonly IArtworkService _ArtworkService;
    private readonly ILogger<ManageOrderArtistController> _logger;
    private readonly ITransactionService _TransactionService;

    public DashboardController(ITransactionService transactionService, ILogger<ManageOrderArtistController> logger,
        IArtworkService artworkService, IArtistService artistService)
    {
        _TransactionService = transactionService;
        _logger = logger;
        _ArtworkService = artworkService;
        _ArtistService = artistService;
    }

    [HttpGet(Name = "Getalltransactionfordashboard")]
    public async Task<ActionResult<ManageOrderArtistController>> GetTransactionsByTimeRange(string timeRange, int page)
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

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("Page.json", true, true)
                .Build();
            var pageSize = int.Parse(configuration.GetSection("Value").Value);
            var transactions = await _TransactionService.GetAll();
            var filteredTransactions = transactions.Where(t => t.CreatedDate >= startDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return Ok(filteredTransactions);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting transactions: {ex.Message}");
            return StatusCode(500, "Internal server error");

        }
    }

    [HttpGet(Name = "GetallArtworkforDashboard")]
    public async Task<ActionResult<ManageOrderArtistController>> GetArtWork(int page)
    {
        try
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("Page.json", true, true)
                .Build();
            var pageSize = int.Parse(configuration.GetSection("Value").Value);
            var transactions = await _ArtworkService.GetAll();
            var Pagefortransaction = transactions
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return Ok(Pagefortransaction);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting transactions: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet(Name = "GetArtistforDashboard")]
    public async Task<ActionResult<ManageOrderArtistController>> GetArtist(int page)
    {
        try
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("Page.json", true, true)
                .Build();
            var pageSize = int.Parse(configuration.GetSection("Value").Value);
            var worker = await _ArtistService.GetAll();
            var Pageforworker = worker.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ;
            return Ok(Pageforworker);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting transactions: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet(Name = "GetArtistforDashboard")]
    public async Task<ActionResult<ManageOrderArtistController>> GetSearchArtist(string name, int page)
    {
        try
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("Page.json", true, true)
                .Build();
            var pageSize = int.Parse(configuration.GetSection("Value").Value);
            var worker = await _ArtistService.GetAll();
            var Pageforworker = worker.Where(w => w.User.Name.Contains(name))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ;
            return Ok(Pageforworker);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting transactions: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}