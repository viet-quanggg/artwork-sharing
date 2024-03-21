using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Helpers;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.Models;
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

    [HttpGet("/artist/{id}")]
    public async Task<IActionResult> GetArtworkByArtist([FromRoute] Guid id, 
        [FromQuery] int pageIndex = 1, [FromQuery] string filter = null, [FromQuery] string orderBy = null)
    {
        if (id == Guid.Empty) return BadRequest();
        int pageSize = 10;

        var paginatedResult = await _artworkService.GetArtworkByArtist(artistId: id, pageIndex: pageIndex, pageSize: pageSize, filter: filter, orderBy: orderBy);
        var paginatedViewModel = new PaginatedResult
        {
            PageIndex = paginatedResult.PageIndex,
            PageSize = paginatedResult.PageSize,
            LastPage = paginatedResult.LastPage,
            IsLastPage = paginatedResult.IsLastPage,
            Total = paginatedResult.Total,
            Data = AutoMapperConfiguration.Mapper.Map<List<ArtworkViewModel>>(paginatedResult.Data)
        };

        return Ok(paginatedViewModel);

    }

    [HttpPost("/user/artist/postartwork")]
    public async Task<IActionResult> CreateNewArtWork([FromBody] CreateArtworkModel artworkModel)
    {        
        if (ModelState.IsValid)
        {
            var artwork = AutoMapperConfiguration.Mapper.Map<Artwork>(artworkModel);
            try
            {
                await _artworkService.Add(artwork);
                return Ok("Artwork created successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
        else
        {           
            return BadRequest(ModelState);
        }
    }
}