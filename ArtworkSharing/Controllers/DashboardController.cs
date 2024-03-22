using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Transactions;

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
    [Authorize(nameof(RoleOfSystem.Admin))]
    [HttpGet("/Transaction",Name = "Getalltransactionfordashboard")]
    public async Task<IActionResult> GetTransactionsByTimeRange(string timeRange, int page)
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

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Page.json");
            var jsonString = await System.IO.File.ReadAllTextAsync(filePath);
            JObject jsonObject = JObject.Parse(jsonString);
            var pageSize = int.Parse(jsonObject["Page"]["Value"].ToString());


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
    [Authorize(nameof(RoleOfSystem.Admin))]
    [HttpGet("/Transaction/Chart", Name = "GetalltransactionforChart")]
    public async Task<IActionResult> GetalltransactionforChart(string timeRange)
    {
        try
        {
           

            DateTime startDate;
            if (timeRange.IsNullOrEmpty())
            {
                startDate = DateTime.Now.AddYears(-1);
            }
            else { 
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
            var transactions = await _TransactionService.GetAudience();
            var filteredTransactions = transactions.Where(t => t.CreatedDate >= startDate);

            return Ok(filteredTransactions);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting transactions: {ex.Message}");
            return StatusCode(500, "Internal server error");

        }
    }
    [Authorize(nameof(RoleOfSystem.Admin))]
    [HttpGet("/Artwork",Name = "GetallArtworkforDashboard")]
    public async Task<ActionResult> GetArtWork()
    {
        try
        {
           
            var transactions = await _ArtworkService.GetAll();
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting Artwork: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    [Authorize(nameof(RoleOfSystem.Admin))]
    [HttpGet("/Artist",Name = "GetArtistforDashboard")]
    public async Task<IActionResult> GetArtistforDashboard()
    {
        try
        {
            var worker = await _ArtistService.GetAll();

            return Ok(worker);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting Artist: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    [Authorize(nameof(RoleOfSystem.Admin))]
    [HttpGet("/Search/{name}", Name = "GetSearchArtist")]
    public async Task<IActionResult> GetSearchArtist(string name, int page)
    {
        try
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Page.json");
            var jsonString = await System.IO.File.ReadAllTextAsync(filePath);
            JObject jsonObject = JObject.Parse(jsonString);
            var pageSize = int.Parse(jsonObject["Page"]["Value"].ToString());


            var worker = await _ArtistService.GetAllField();
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
    [Authorize(nameof(RoleOfSystem.Admin))]
    [HttpGet("/GetNameArtist/{id}", Name = "GetNameArtist")]
    public async Task<IActionResult> GetNameArtist(Guid id)
    {
        try
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Page.json");
            var jsonString = await System.IO.File.ReadAllTextAsync(filePath);
            JObject jsonObject = JObject.Parse(jsonString);
            var pageSize = int.Parse(jsonObject["Page"]["Value"].ToString());

            var worker = await _ArtistService.GetnameArtist(id);
            var Pageforworker = worker.User.Name;

            return Ok(Pageforworker);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting transactions: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    [Authorize(nameof(RoleOfSystem.Admin))]
    [HttpGet("/SearchArtwork", Name = "GetSearchArtwork")]
    public async Task<IActionResult> GetSearchArtwork(Guid id)
    {
        try
        {

            var transactions = await _ArtworkService.GetMediaContentforArtwork(id);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting transactions: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    [Authorize(nameof(RoleOfSystem.Admin))]
    [HttpGet("/ArtworkbyId", Name = "GetArtworkbyId")]
    public async Task<ActionResult> GetArtworkbyId(Guid id)
    {
        try
        {
            var transactions = await _ArtworkService.GetOne(id);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting Artwork: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}