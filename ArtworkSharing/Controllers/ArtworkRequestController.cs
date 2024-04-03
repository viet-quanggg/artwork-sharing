using System.Security.Claims;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Service.AutoMappings;
using ArtworkSharing.Extensions;
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

    [Authorize]
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
    [Authorize]
    [HttpGet("/GetArtworkRequestsByArtist")]
    public async Task<IActionResult> GetArtworkRequestsByArtist()
    {
        var id = HttpContext.Items["UserId"];
        if (id == null) return Unauthorized();

        Guid uid = Guid.Parse(id + "");

        if (uid == Guid.Empty) return Unauthorized();
        if (uid == null) return BadRequest();
        return Ok(await _requestService.GetArtworkRequestByArtist(uid));
    }
    
    [Authorize]
    [HttpPut("/CancelArtworkRequestByArtist/{artworkRequestId}")]
    public async Task<IActionResult> CancelArtworkRequestByArtist(Guid artworkRequestId)
    {
        if (artworkRequestId == null) return BadRequest();
        return Ok(await _requestService.CancelArtworkRequestByArtist(artworkRequestId));
        
    }
    
    [Authorize]
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
    [Authorize]
    [HttpGet("/GetArtworkRequestsByUser")]
    public async Task<IActionResult> GetArtworkRequestsByUser()
    {
        
        try
        {
            var id = HttpContext.Items["UserId"];
            if (id == null) return Unauthorized();

            Guid uid = Guid.Parse(id + "");

            if (uid == Guid.Empty) return Unauthorized();
            if (uid == null) return BadRequest();
            
            var list = await _requestService.GetArtworkRequestsByUser(uid);
            return Ok(list);

        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [Authorize]
    [HttpPut("/CancelArtworkRequestByUser/{artworkRequestId}")]
    public async Task<IActionResult> CancelArtworkRequestByUser(Guid artworkRequestId)
    {
        if (artworkRequestId == null) return BadRequest();
        return Ok(await _requestService.CancelArtworkRequestByUser(artworkRequestId));
        
    }
    
    [Authorize]
    [HttpPut("/ChangeStatusAfterDeposit/")]
    public async Task<IActionResult> ChangeStatusAfterDeposit(TransactionViewModel tvm)
    {
        if (tvm == null) return BadRequest();
        return Ok(await _requestService.ChangeStatusAfterDeposit(tvm));
        
    }
    
    [Authorize]
    [HttpPost("createartworkrequest")]
    public async Task<IActionResult> CreateArtworkRequest(CreateArtworkRequestModel cam)
    {
        try
        {
            var id = HttpContext.Items["UserId"];
            if (id == null) return Unauthorized();

            Guid uid = Guid.Parse(id + "");

            if (uid == Guid.Empty) return Unauthorized();
            if (uid == null) return BadRequest();
            
            cam.AudienceId = uid;
            return Ok(await _requestService.CreateArtworkRequest(cam));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    //User Controller

   

    
}