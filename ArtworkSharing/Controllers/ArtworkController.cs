using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.Models;
using ArtworkSharing.Core.ViewModels.Artworks;
using ArtworkSharing.Service.AutoMappings;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ArtworkSharing.Extensions;

namespace ArtworkSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArtworkController : ControllerBase
{
    private readonly IArtworkService _artworkService;
    private readonly IFireBaseService _fireBaseService;
    private readonly IWatermarkService _watermarkService;
    private readonly IArtistService _artistService;
    private IMapper mapper;

    public ArtworkController(IArtworkService artworkService,
        IFireBaseService fireBaseService,
        IWatermarkService watermarkService,
        IArtistService artistService)
    {
        _artworkService = artworkService;
        _fireBaseService = fireBaseService;
        _watermarkService = watermarkService;
        _artistService = artistService;
    }

    /// <summary>
    ///     Get artwork with filter model
    /// </summary>
    /// <param name="browseArtwork"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetArtworks(Guid? ArtistId, string? Name, string? Description, bool IsPopular, bool IsAscRecent, int PageIndex, Guid? categoryId)
    {
        var PageSize = PageIndex > 0 ? 10 * PageIndex : 10;
        PageIndex = 0;
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
        var a = await _artworkService.GetArtworks(browseArtwork);
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
    [Authorize]
    public async Task<IActionResult> CreateNewArtWork([FromForm] CreateArtworkModel artworkModel)
    {        
        if (ModelState.IsValid)
        {
            var id = HttpContext.Items["UserId"];
            if (id == null) return Unauthorized();

            Guid uid = Guid.Parse(id + "");

            if (uid == Guid.Empty) return Unauthorized();
            var artist = await _artistService.GetArtistByUserId(uid);
            if (artist == null)
            {
                return BadRequest("You are not an artist");
            }
            artworkModel.ArtistId = artist.Id;
            var artwork = AutoMapperConfiguration.Mapper.Map<Artwork>(artworkModel);
            try
            {
                artwork.MediaContents = await MapMediaContents(artworkModel.MediaContents);

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


    private async Task<List<MediaContent>> MapMediaContents(List<IFormFile> mediaContents)
    {
        var listToReturn = new List<MediaContent>();
      
        var mediaList = await _fireBaseService.UploadMultiImagesAsync(mediaContents);

        foreach (var media in mediaList)
        {
            var mediaReturn = new MediaContent
            {
                Media = await _watermarkService.AddWatermarkAsync(media),
                MediaWithoutWatermark = media
            };
            listToReturn.Add(mediaReturn);
        }
        return listToReturn;
    }

}