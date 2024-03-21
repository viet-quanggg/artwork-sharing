using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Helpers;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Artworks;
using ArtworkSharing.Service.AutoMappings;
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
    public async Task<IActionResult> GetArtworks(Guid? ArtistId, string? Name, string? Description, bool IsPopular, bool IsAscRecent, int PageIndex, Guid? categoryId)
    {
        var PageSize = 10;
        BrowseArtworkModel browseArtwork = new BrowseArtworkModel
        {
            ArtistId = ArtistId,
            Description = Description,
            IsAscRecent = IsAscRecent,
            IsPopular = IsPopular,
            Name = Name,
            PageIndex = PageIndex >= 0 ? PageIndex : 0,
            PageSize = PageSize > 0 ? PageSize : 10,
            CategoryId = categoryId
        };
        return Ok(AutoMapperConfiguration.Mapper.Map<List<ArtworkViewModel>>(await _artworkService.GetArtworks(browseArtwork)));
    }


    /// <summary>
    /// Get artwork by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArtwork([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest();

        var rs = await _artworkService.GetOne(id);

        return Ok(AutoMapperConfiguration.Mapper.Map<ArtworkViewModel>(rs));
    }
}