using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpPost("follow/{followUserId}")]
    
    public async Task<IActionResult> FollowCreator(Guid followUserId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid currentUserId = new Guid(userIdClaim?.Value);
        if (string.IsNullOrEmpty(currentUserId.ToString()) || string.IsNullOrEmpty(followUserId.ToString()))
            return BadRequest();
        if (currentUserId == followUserId)
            return BadRequest("You can't follow yourself");

        if (await _followService.IsFollowing(currentUserId, followUserId))
            return BadRequest("You are already following this user");
        try
        {
            await _followService.FollowUser(currentUserId, followUserId);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
        

        return Ok();
    }

    [HttpPost("unfollow/{followUserId}")]
   
    public async Task<IActionResult> UnFollowCreator(Guid followUserId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid currentUserId = new Guid(userIdClaim?.Value);
        if (string.IsNullOrEmpty(currentUserId.ToString()) || string.IsNullOrEmpty(followUserId.ToString()))
            return BadRequest();

        if (currentUserId == followUserId)
            return BadRequest();

        if (!await _followService.IsFollowing(currentUserId, followUserId))
            return BadRequest("You are not already following this user");
        try
        {
            await _followService.UnFollowUser(currentUserId, followUserId);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("isFollowed/{followUserId}")]
 
    public async Task<IActionResult> IsFollowed(Guid followUserId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid currentUserId = new Guid(userIdClaim?.Value);
        if (string.IsNullOrEmpty(currentUserId.ToString()) || string.IsNullOrEmpty(followUserId.ToString()))
            return BadRequest();

        var isFollowed = await _followService.IsFollowing(currentUserId, followUserId);

        return Ok(isFollowed);
    }   
}