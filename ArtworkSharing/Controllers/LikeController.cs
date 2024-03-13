using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Likes;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly ILikeService _likeService;

    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    /// <summary>
    ///     Get like by artworkId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLikeByArtworkId(Guid id)
    {
        if (id == Guid.Empty) return BadRequest();

        return Ok(await _likeService.GetLikeByArtworkId(id));
    }

    /// <summary>
    ///     Like and unlike artwork
    /// </summary>
    /// <param name="likeModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> LikeArtwork(LikeModel likeModel)
    {
        if (likeModel.UserId == Guid.Empty || likeModel.ArtworkId == Guid.Empty) return BadRequest();
        var rs = await _likeService.Update(likeModel);
        return rs != null
            ? Ok(rs)
            : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Action failed" });
    }
}