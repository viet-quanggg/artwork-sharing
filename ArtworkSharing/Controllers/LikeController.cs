﻿using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Likes;
using ArtworkSharing.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtworkSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly ILikeService _likeService;

    public LikeController(ILikeService likeService, IConfiguration configuration)
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
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> LikeArtwork(LikeModel likeModel)
    {
        var id = HttpContext.Items["UserId"];
        if (id == null) return Unauthorized();

        Guid uid = Guid.Parse(id + "");

        if (uid == Guid.Empty) return Unauthorized();

        if (likeModel.ArtworkId == Guid.Empty) return BadRequest();

        var rs = await _likeService.Update(likeModel.ArtworkId, uid);
        return rs != null
            ? Ok(rs)
            : StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Action failed" });
    }

    /// <summary>
    /// Check like by userid and artworkid
    /// </summary>
    /// <param name="artworkId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> CheckLike(Guid artworkId)
    {
        var id = HttpContext.Items["UserId"];
        if (id == null) return Unauthorized();

        Guid uid = Guid.Parse(id + "");

        if (uid == Guid.Empty) return Unauthorized();

        if (artworkId == Guid.Empty) return BadRequest();
        var rs = await _likeService.CheckLike(artworkId, uid);

        return Ok(rs);
    }
}