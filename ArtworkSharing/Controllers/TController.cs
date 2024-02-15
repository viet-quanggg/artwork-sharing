using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ArtworkSharing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TController : Controller
    {
        private readonly IArtistService _ArtistService;
        private readonly IArtworkService _ArtworkService;
        private readonly ILikeService _LikeService;
        private readonly IRatingService _RatingService;
        private readonly ICommentService _CommentService;

        private readonly ILogger<TController> _logger;
        public TController(IArtistService artistService, IArtworkService artworkService,ILikeService likeService,IRatingService ratingService,ICommentService commentService, ILogger<TController> logger)
        {
            _ArtistService = artistService;
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
        public async Task<ActionResult> Add(Guid artistId, [FromBody] Artwork artwork)
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
        public async Task<ActionResult> Update([FromBody] Artwork artworkInput)
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
        public async Task<ActionResult> Delete(Guid artworkId)
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
        public async Task<ActionResult> AddArtworks(Guid artistId, [FromBody] List<Artwork> artworks)
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

    }
}
