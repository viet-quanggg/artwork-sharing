using System.Collections;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Artworks;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IArtworkService _artworkService;
    public AdminController(IArtworkService artworkService)
    {
        _artworkService = artworkService;
    }
    
    [HttpGet("artworks")]
    public async Task<ActionResult> GetArtworks(int pageNumber, int pageSize)
    {
        try
        {
            var artworkList = await _artworkService.GetArtworksAdmin(pageNumber, pageSize);
            return Ok(artworkList);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPut("disableArtwork/{artworkId}")]
    public async Task<ActionResult> DisableArtwork(Guid artworkId)
    {
        try
        {
            await _artworkService.ChangeArtworkStatus_DisableAsync(artworkId);
            return Ok();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    

    [HttpPut("{artworkId}")]
    public async Task<IActionResult> UpdateArtworkAdmin([FromRoute] Guid artworkId, ArtworkUpdateModelAdmin aump)
    {
        if (artworkId == Guid.Empty || aump == null) return BadRequest(new { Message = "Artwork not found!" });
        return Ok(await _artworkService.UpdateAdmin(artworkId, aump));
    }
    
    [HttpDelete("{artworkId}")]
    public async Task<IActionResult> DeleteArtworkAdmin(Guid artworkId)
    {
        if (artworkId == Guid.Empty) return BadRequest(new { Message = "Artwork not found!" });
        return Ok(await _artworkService.DeleteArtworkAdmin(artworkId));
    }

    [HttpPost]
    public async Task<ActionResult> CreateArtworkAdmin([FromRoute] Guid artistId, ArtworkCreateModelAdmin acmd)
    {
        try
        {
            var artwork = AutoMapperConfiguration.Mapper.Map<Artwork>(acmd);
            
            return Ok(await _artworkService.CreateArtworkAdmin(artistId,acmd));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    
}