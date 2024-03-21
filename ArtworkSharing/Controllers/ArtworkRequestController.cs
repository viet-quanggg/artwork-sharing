using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[ApiController]
[Route("api/artworkrequest")]
public class ArtworkRequestController : Controller
{
    private readonly IArtworkRequestService _requestService;

    public ArtworkRequestController(IArtworkRequestService artworkRequestService)
    {
        _requestService = artworkRequestService;
    }
    
    [HttpGet("getartworkRequest")]
    public async Task<IActionResult> GetArtworkRequest(Guid artworkRequestId)
    {
        try
        {
            var artworkRequest = await _requestService.GetArtworkService(artworkRequestId);
            return Ok(artworkRequest);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllArtworkRequests(int pageNumber, int pageSize)
    {
        try
        {
            var requestList = await _requestService.GetArtworkServices(pageNumber, pageSize);
            return Ok(requestList);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    //Artist Controllers
    [HttpGet("/GetArtworkRequestsByArtist/{artistId}")]
    public async Task<IActionResult> GetArtworkRequestsByArtist(Guid artistId)
    {
        if (artistId == null) return BadRequest();
        return Ok(await _requestService.GetArtworkRequestByArtist(artistId));
    }
    
    [HttpPut("/CancelArtworkRequestByArtist/{artworkRequestId}")]
    public async Task<IActionResult> CancelArtworkRequestByArtist(Guid artworkRequestId)
    {
        if (artworkRequestId == null) return BadRequest();
        return Ok(await _requestService.CancelArtworkRequestByArtist(artworkRequestId));
        
    }
    
    [HttpPut("/AcceptArtworkRequestByArtist/{artworkRequestId}")]
    public async Task<IActionResult> AcceptArtworkRequestByArtist(Guid artworkRequestId)
    {
        if (artworkRequestId == null) return BadRequest();
        return Ok(await _requestService.AcceptArtworkRequestByArtist(artworkRequestId));
        
    }
    //Artist Controllers

    
    
    //User Controllers
    [HttpGet("/GetArtworkRequestsByUser/{userId}")]
    public async Task<IActionResult> GetArtworkRequestsByUser(Guid userId)
    {
        if (userId == null) return BadRequest();
        return Ok(await _requestService.GetArtworkRequestsByUser(userId));
    }

    [HttpPut("/CancelArtworkRequestByUser/{artworkRequestId}")]
    public async Task<IActionResult> CancelArtworkRequestByUser(Guid artworkRequestId)
    {
        if (artworkRequestId == null) return BadRequest();
        return Ok(await _requestService.CancelArtworkRequestByUser(artworkRequestId));
        
    }
    
    [HttpPost("createartworkrequest")]
    public async Task<IActionResult> CreateArtworkRequest(CreateArtworkRequestModel cam)
    {
        try
        {
            return Ok(await _requestService.CreateArtworkRequest(cam));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    //User Controller

   

    
}