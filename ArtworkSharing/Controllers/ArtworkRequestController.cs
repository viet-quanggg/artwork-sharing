using System.Security.Claims;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(nameof(RoleOfSystem.Admin))]
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
    [Authorize(nameof(RoleOfSystem.Artist))]
    [HttpGet("/GetArtworkRequestsByArtist")]
    public async Task<IActionResult> GetArtworkRequestsByArtist()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid currentUserId = new Guid(userIdClaim?.Value);
        if (currentUserId == null) return BadRequest();
        return Ok(await _requestService.GetArtworkRequestByArtist(currentUserId));
    }
    
    [Authorize(nameof(RoleOfSystem.Artist))]
    [HttpPut("/CancelArtworkRequestByArtist/{artworkRequestId}")]
    public async Task<IActionResult> CancelArtworkRequestByArtist(Guid artworkRequestId)
    {
        if (artworkRequestId == null) return BadRequest();
        return Ok(await _requestService.CancelArtworkRequestByArtist(artworkRequestId));
        
    }
    
    [Authorize(nameof(RoleOfSystem.Artist))]
    [HttpPut("/AcceptArtworkRequestByArtist/{artworkRequestId}")]
    public async Task<IActionResult> AcceptArtworkRequestByArtist(Guid artworkRequestId)
    {
        if (artworkRequestId == null) return BadRequest();
        return Ok(await _requestService.AcceptArtworkRequestByArtist(artworkRequestId));
        
    }
    [HttpPost("/CommitArtworkByArtist/{artworkRequestId}")]
    public async Task<IActionResult> CommitArtworkByArtist(Guid artworkRequestId, CommitArtworkRequestModel uarm)
    {
        if (artworkRequestId == null) return BadRequest();
        return Ok(await _requestService.CommitArtworkRequest(artworkRequestId,uarm));

    }
    //Artist Controllers



    //User Controllers
    [Authorize(nameof(RoleOfSystem.Audience))]
    [HttpGet("/GetArtworkRequestsByUser")]
    public async Task<IActionResult> GetArtworkRequestsByUser()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid currentUserId = new Guid(userIdClaim?.Value);
        if (currentUserId == null) return BadRequest();
        try
        {
            var list = await _requestService.GetArtworkRequestsByUser(currentUserId);
            return Ok(list);

        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [Authorize(nameof(RoleOfSystem.Audience))]
    [HttpPut("/CancelArtworkRequestByUser/{artworkRequestId}")]
    public async Task<IActionResult> CancelArtworkRequestByUser(Guid artworkRequestId)
    {
        if (artworkRequestId == null) return BadRequest();
        return Ok(await _requestService.CancelArtworkRequestByUser(artworkRequestId));
        
    }
    
    [Authorize(nameof(RoleOfSystem.Audience))]
    [HttpPut("/ChangeStatusAfterDeposit/")]
    public async Task<IActionResult> ChangeStatusAfterDeposit(TransactionViewModel tvm)
    {
        if (tvm == null) return BadRequest();
        return Ok(await _requestService.ChangeStatusAfterDeposit(tvm));
        
    }
    
    [Authorize(nameof(RoleOfSystem.Audience))]
    [HttpPost("createartworkrequest")]
    public async Task<IActionResult> CreateArtworkRequest(CreateArtworkRequestModel cam)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid currentUserId = new Guid(userIdClaim?.Value);
        try
        {
            cam.AudienceId = currentUserId;
            return Ok(await _requestService.CreateArtworkRequest(cam));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    //User Controller

   

    
}