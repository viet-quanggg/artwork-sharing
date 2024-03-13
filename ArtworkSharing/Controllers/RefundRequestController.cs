using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.RefundRequests;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[Route("[controller]")]
[ApiController]
public class RefundRequestController : ControllerBase
{
    private readonly IRefundRequestService _refundRequestService;

    public RefundRequestController(IRefundRequestService refundRequestService)
    {
        _refundRequestService = refundRequestService;
    }

    [HttpGet]
    public async Task<ActionResult<List<RefundRequestViewModel>>> GetAllRefundRequests()
    {
        var refundRequests = await _refundRequestService.GetAll();
        return Ok(refundRequests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RefundRequestViewModel>> GetRefundRequest(Guid id)
    {
        var refundRequest = await _refundRequestService.GetRefundRequest(id);
        if (refundRequest == null) return NotFound();
        return Ok(refundRequest);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRefundRequest(Guid id, UpdateRefundRequestModel refundRequestInput)
    {
        try
        {
            await _refundRequestService.UpdateRefundRequest(id, refundRequestInput);
            return Ok(refundRequestInput);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateRefundRequest(RefundRequest refundRequestInput)
    //{
    //    try
    //    {
    //        await _refundRequestService
    //        return CreatedAtAction(nameof(GetRefundRequest), new { id = refundRequestInput.Id }, refundRequestInput);
    //    }
    //    catch (Exception)
    //    {
    //        return StatusCode(500); // Internal Server Error
    //    }
    //}

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRefundRequest(Guid id)
    {
        try
        {
            await _refundRequestService.DeleteRefundRequest(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}