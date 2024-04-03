﻿using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[ApiController]
[Route("[controller]")]
public class ManageOrderArtistController : ControllerBase
{
    private readonly IArtistPackageService _ArtistService;
    private readonly IArtworkService _ArtworkService;

    /* private readonly ILikeService _LikeService;
     private readonly IRatingService _RatingService;
     private readonly ICommentService _CommentService;*/

    private readonly ILogger<ManageOrderArtistController> _logger;

    private readonly ITransactionService _TransactionService;

    public ManageOrderArtistController(IArtistPackageService artistService, IArtworkService artworkService,
        ITransactionService transactionService, ILogger<ManageOrderArtistController> logger)
    {
        _ArtistService = artistService;
        _TransactionService = transactionService;
        _ArtworkService = artworkService;
        _logger = logger;
    }


    [HttpGet("ArtistId", Name = "GetCompleteTransaction")]
    public async Task<IActionResult> GetTransactionofArtist(Guid ArtistId, int page)
    {
        try
        {
            var Artist = _ArtistService.GetOne(ArtistId);
            if (Artist != null)
            {
                var Artworks = await _ArtworkService.GetAll();
                var getArtwork = Artworks.Where(r => r.ArtistId == ArtistId).ToList();
                var trans = await _TransactionService.GetAll();
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("Page.json", true, true)
                    .Build();
                var pageSize = int.Parse(configuration.GetSection("Value").Value);
                var transofArtwork = trans.Where(t => t.ArtworkId == ArtistId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                return Ok(trans);
            }

            return NotFound("Artist not found");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding Artwork: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}