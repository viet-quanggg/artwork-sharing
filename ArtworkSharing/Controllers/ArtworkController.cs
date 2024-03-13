using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Helpers;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Artworks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArtworkController : ControllerBase
{
    private readonly IArtworkService _artworkService;
    private IMapper mapper;

    public ArtworkController(IArtworkService artworkService)
    {
        _artworkService = artworkService;
    }

    /// <summary>
    ///     Get artwork with filter model
    /// </summary>
    /// <param name="browseArtwork"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetArtworks(BrowseArtworkModel browseArtwork)
    {
        return Ok(PaginationHelper
            .BuildPaginatedResult<Artwork, ArtworkViewModel>
            (mapper, (await _artworkService.GetArtworks(browseArtwork)).AsQueryable(), browseArtwork.PageSize,
                browseArtwork.PageIndex));
        ;
    }
}