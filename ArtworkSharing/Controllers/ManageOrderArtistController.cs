using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Artworks;
using Microsoft.AspNetCore.Mvc;


namespace ArtworkSharing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManageOrderArtistController : ControllerBase
    {
        private readonly IArtistPackageService _ArtistService;
        private readonly IArtworkService _ArtworkService;

        private readonly ITransactionService _TransactionService;

        /* private readonly ILikeService _LikeService;
         private readonly IRatingService _RatingService;
         private readonly ICommentService _CommentService;*/

        private readonly ILogger<ManageOrderArtistController> _logger;
        public ManageOrderArtistController(IArtistPackageService artistService, IArtworkService artworkService,ITransactionService transactionService, ILogger<ManageOrderArtistController> logger)
        {
            _ArtistService = artistService;
            _TransactionService = transactionService;
            _ArtworkService = artworkService;
            _logger = logger;
        }
       
        
        [HttpGet("ArtistId",Name ="GetCompleteTransaction")]
        public async Task<IActionResult> GetTransactionofArtist(Guid ArtistId)
        {
            try
            {
                var Artist = _ArtistService.GetOne(ArtistId);
                if (Artist != null)
                {
                  var Artworks = await _ArtworkService.GetAll();
                    var getArtwork = Artworks.Where(r => r.ArtistId == ArtistId).ToList();
                    var trans = await _TransactionService.GetAll();
                    var transofArtwork = trans.Where(t => t.ArtworkId == ArtistId).ToList();
                    return Ok(trans);
                }
                return NotFound("Artist not found");
            }
            catch(Exception ex) {
                _logger.LogError($"Error adding Artwork: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
