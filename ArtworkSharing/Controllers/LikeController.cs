using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Likes;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLikeByArtworkId(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            return Ok(await _likeService.GetLikeByArtworkId(id));
        }

        [HttpPost]
        public async Task<IActionResult> LikeArtwork(LikeModel cm)
        {
            if (cm.UserId == Guid.Empty || cm.ArtworkId == Guid.Empty) return BadRequest();
            var rs = await _likeService.Update(cm);
            return rs != null ? Ok(rs) : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Action failed" });
        }
    }
}
