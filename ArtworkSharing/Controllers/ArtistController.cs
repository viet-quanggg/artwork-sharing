using ArtworkSharing.Core.Interfaces.Repositories;
using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[ApiController]
[Route("api/artist")]
public class ArtistController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet("/GetArtistProfile/{artistId}")]
    public async Task<IActionResult> GetArtistProfile(Guid artistId)
    {
        if (artistId == null) return BadRequest();
        return Ok(await _artistService.GetArtistProfile(artistId));
    }
    
}