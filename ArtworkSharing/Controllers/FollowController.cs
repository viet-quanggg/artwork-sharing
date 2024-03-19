using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FollowController : ControllerBase
{
    private readonly IFollowService _followService;

    public FollowController(IFollowService followService)
    {
        _followService = followService;
    }

    [HttpPost("follow")]
    public async Task<IActionResult> FollowCreator(Guid currentUserId, Guid followUserId)
    {
        if (string.IsNullOrEmpty(currentUserId.ToString()) || string.IsNullOrEmpty(followUserId.ToString()))
            return BadRequest();
        if (currentUserId == followUserId)
            return BadRequest("You can't follow yourself");

        if (await _followService.IsFollowing(currentUserId, followUserId))
            return BadRequest("You are already following this user");

        await _followService.FollowUser(currentUserId, followUserId);

        return Ok();
    }

    [HttpPost("unfollow")]
    public async Task<IActionResult> UnFollowCreator(Guid currentUserId, Guid followUserId)
    {
        if (string.IsNullOrEmpty(currentUserId.ToString()) || string.IsNullOrEmpty(followUserId.ToString()))
            return BadRequest();

        if (currentUserId == followUserId)
            return BadRequest();

        if (!await _followService.IsFollowing(currentUserId, followUserId))
            return BadRequest("You are not already following this user");

        await _followService.UnFollowUser(currentUserId, followUserId);

        return Ok();
    }
}