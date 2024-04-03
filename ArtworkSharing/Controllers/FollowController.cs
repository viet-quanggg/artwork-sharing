using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ArtworkSharing.Extensions;

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

    [Authorize]
    [HttpPost("follow/{followUserId}")]
    public async Task<IActionResult> FollowCreator(Guid followUserId)
    {
        var id = HttpContext.Items["UserId"];
        if (id == null) return Unauthorized();

        Guid uid = Guid.Parse(id + "");

        if (uid == Guid.Empty) return Unauthorized();
        if (string.IsNullOrEmpty(uid.ToString()) || string.IsNullOrEmpty(followUserId.ToString()))
            return BadRequest();
        if (uid == followUserId)
            return BadRequest("You can't follow yourself");

        if (await _followService.IsFollowing(uid, followUserId))
            return BadRequest("You are already following this user");
        try
        {
            await _followService.FollowUser(uid, followUserId);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
        

        return Ok();
    }

    [Microsoft.AspNetCore.Authorization.Authorize]
    [HttpPost("unfollow/{followUserId}")]
    public async Task<IActionResult> UnFollowCreator(Guid followUserId)
    {
        var id = HttpContext.Items["UserId"];
        if (id == null) return Unauthorized();

        Guid uid = Guid.Parse(id + "");

        if (uid == Guid.Empty) return Unauthorized();
        if (string.IsNullOrEmpty(uid.ToString()) || string.IsNullOrEmpty(followUserId.ToString()))
            return BadRequest();

        if (uid == followUserId)
            return BadRequest();

        if (!await _followService.IsFollowing(uid, followUserId))
            return BadRequest("You are not already following this user");
        try
        {
            await _followService.UnFollowUser(uid, followUserId);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }

        return Ok();
    }

    [Authorize]
    [HttpGet("isFollowed/{followUserId}")]    
    public async Task<IActionResult> IsFollowed(Guid followUserId)
    {
        var id = HttpContext.Items["UserId"];
        if (id == null) return Unauthorized();

        Guid uid = Guid.Parse(id + "");

        if (uid == Guid.Empty) return Unauthorized();
        if (string.IsNullOrEmpty(uid.ToString()) || string.IsNullOrEmpty(followUserId.ToString()))
            return BadRequest();

        var isFollowed = await _followService.IsFollowing(uid, followUserId);

        return Ok(isFollowed);
    }   
}