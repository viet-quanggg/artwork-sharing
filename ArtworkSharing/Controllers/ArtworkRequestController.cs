using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task<ActionResult> GetAllArtworkRequests()
    {
        try
        {
            var requestList = await _requestService.GetArtworkServices();
            return Ok(requestList);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    [HttpGet("{artworkRequestId}")]
    public async Task<ActionResult> GetArtworkRequest()
    {
        try
        {
            var artworkRequest = await _requestService.GetArtworkServices();
            return Ok(artworkRequest);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost("createartworkrequest")]
    public async Task<ActionResult> CreateArtworkRequest([FromBody] CreateArtworkRequestModel cam)
    {
        try
        {
            var createRequest = AutoMapperConfiguration.Mapper.Map<ArtworkService>(cam);
             _requestService.CreateArtworkRequest(createRequest);
            return Ok(createRequest);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}