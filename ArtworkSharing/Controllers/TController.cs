using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.Http;
using System.Xml;

namespace ArtworkSharing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TController : Controller
    {
        private readonly IArtistService _ArtistService;
        private readonly IArtworkService _ArtworkService;

        private readonly ITransactionService _TransactionService;

        /* private readonly ILikeService _LikeService;
         private readonly IRatingService _RatingService;
         private readonly ICommentService _CommentService;*/

        private readonly ILogger<TController> _logger;
        public TController(IArtistService artistService, IArtworkService artworkService,ITransactionService transactionService, ILogger<TController> logger)
        {
            _ArtistService = artistService;
            _TransactionService = transactionService;
            _ArtworkService = artworkService;
            _logger = logger;
        }
        [HttpGet("{entityId}", Name = "GetArtworkofArtist")]
        public async Task<ActionResult<TController>> GetCombinedEntityById(Guid entityId)
        {
            try
            {
                var artists = await _ArtistService.GetOne(entityId);
                if (artists == null)
                {
                    return NotFound("Artist not found");
                }
                var artworks = artists.Artworks;
                return Ok(artworks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting Artwork: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("{artistId}", Name = "AddArtwork")]
        public async Task<IActionResult> Add(Guid artistId, [FromBody] Artwork artwork)
        {
            try
            {
                var artist = await _ArtistService.GetOne(artistId);
                if (artist == null)
                {
                    return NotFound("Artist not found");
                }
                artwork.Artist = artist;
                await _ArtworkService.Add(artwork);
                return CreatedAtRoute("GetArtworkById", new { artworkId = artwork.Id }, artwork);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding Artwork: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut(Name = "EditArtwork")]
        public async Task<IActionResult> Update([FromBody] Artwork artworkInput)
        {
            try
            {
                var existArtwork = await _ArtworkService.GetOne(artworkInput.Id);
                if (existArtwork == null)
                {
                    return NotFound("Artwork not found");
                }
                existArtwork.Name = artworkInput.Name;
                existArtwork.Price = artworkInput.Price;
                existArtwork.Description = artworkInput.Description;
                await _ArtworkService.Update(existArtwork);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Artwork: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{artworkId}", Name = "DeleteArtwork")]
        public async Task<IActionResult> Delete(Guid artworkId)
        {
            try
            {
                var existingArtwork = await _ArtworkService.GetOne(artworkId);
                if (existingArtwork == null)
                {
                    return NotFound();
                }

                await _ArtworkService.Delete(artworkId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting Artwork: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("{artistId}", Name = "AddlistArtworks")]
        public async Task<IActionResult> AddArtworks(Guid artistId, [FromBody] List<Artwork> artworks)
        {
            try
            {
                var artist = await _ArtistService.GetOne(artistId);
                if (artist == null)
                {
                    return NotFound("Artist not found");
                }

                foreach (var artwork in artworks)
                {
                    artwork.Artist = artist;
                    await _ArtworkService.Add(artwork);
                }

                return CreatedAtRoute("GetArtworksByArtist", new { artistId = artist.Id }, artworks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding artworks: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet( Name = "Getalltransactionfordashboard")]
        public async Task<ActionResult<TController>> GetTransactionsByTimeRange(string timeRange)
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
        public async Task<ActionResult<TController>> GetArtWork()
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
        public async Task<ActionResult<TController>> GetWorker(Guid entityId)
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
        [HttpPost( Name = "Save Page")]
        public async Task<IActionResult> SaveApiResponseToJsonFile([FromForm] int Page)
        {

            try
            {

                IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("Page.json", true, true)
                .Build();
                var num = configuration.GetSection("Page").Value;
                IConfigurationRoot _configuration = (IConfigurationRoot)configuration;
                if (num.IsNullOrEmpty())
                {
                     _configuration.GetSection("Page").Value = Page.ToString();
                     var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Page.json");
                     System.IO.File.WriteAllText(filePath , JsonConvert.SerializeObject(_configuration.AsEnumerable(), Newtonsoft.Json.Formatting.Indented));

                }



                return Ok("API response saved to JSON file.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
