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
}