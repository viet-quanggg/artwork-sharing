using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.RefundRequests;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;
[ApiController]
[Route("api/refundrequest")]
public class RefundRequestController : Controller
{
    private IRefundRequestService _requestService;
    public RefundRequestController(IRefundRequestService refundRequestService)
    {
        _requestService = refundRequestService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllRefunds()
    {
        try
        {
            var refundList = await _requestService.GetAll();
            return Ok(refundList);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpGet("{refundId}")]
    public async Task<ActionResult> GetRefund(Guid refundId)
    {
        try
        {
            var refund = await _requestService.GetRefundRequest(refundId);
            return Ok(refund);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpPost("createRefund")]
    public async Task<ActionResult> CreateRefund([FromBody] CreateRefundRequestModel crrm)
    {
        try
        {
            //var createRefund = AutoMapperConfiguration.Mapper.Map<RefundRequest>(crrm);
            await _requestService.CreateRefundRequest(crrm);
            return Ok(crrm);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }



}