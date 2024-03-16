using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.RefundRequests;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

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


    //[HttpGet]
    //public async Task<ActionResult<List<RefundRequestViewModel>>> GetAllRefundRequests()
    //{
    //    var refundRequests = await _refundRequestService.GetAll();
    //    return Ok(refundRequests);
    //}
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetRefundRequestCount()
    {
        try
        {
            int count = await _refundRequestService.Count();
            return Ok(count);
        }
        catch (Exception)
        {
            return StatusCode(500); // Lỗi máy chủ nội bộ
        }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<List<RefundRequestViewModel>>> GetDetailPackage(Guid id)
    {
        try
        {
            Expression<Func<RefundRequest, bool>> filter = refundRequest => refundRequest.Id == id;


            // Khởi tạo hàm sắp xếp giảm dần theo thời gian
            Func<IQueryable<RefundRequest>, IOrderedQueryable<RefundRequest>> orderBy = q => q.OrderByDescending(p => p.RefundRequestDate);

            string includeProperties = "Transaction";

            var packages = _refundRequestService.Get(filter, null, includeProperties, 1, 3);
            // Chuyển đổi sang view model nếu cần
            // var packageViewModels = packages.Select(p => new PackageViewModel { ... }).ToList();
            return Ok(packages);
        }
        catch (Exception)
        {
            return StatusCode(500); // Lỗi máy chủ nội bộ
        }
    }

    [HttpGet(Name = "GetRefundRequestWithPaging")]
    public async Task<ActionResult<List<RefundRequestViewModel>>> GetPackageWithPaging(
 [FromQuery] int? pageIndex = null,
 [FromQuery] int? pageSize = null)
    {
        try
        {
            Expression<Func<RefundRequest, bool>> filter = null;

            // Khởi tạo hàm sắp xếp giảm dần theo thời gian
            Func<IQueryable<RefundRequest>, IOrderedQueryable<RefundRequest>> orderBy = q => q.OrderByDescending(p => p.RefundRequestDate);

            string includeProperties = "Transaction";

            var packages = _refundRequestService.Get(filter, orderBy, includeProperties, pageIndex, pageSize);
            // Chuyển đổi sang view model nếu cần
            // var packageViewModels = packages.Select(p => new PackageViewModel { ... }).ToList();
            return Ok(packages);
        }
        catch (Exception)
        {
            return StatusCode(500); // Lỗi máy chủ nội bộ
        }
    }


    //[HttpGet("{id}")]
    //public async Task<ActionResult<RefundRequestViewModel>> GetRefundRequest(Guid id)
    //{
    //    var refundRequest = await _refundRequestService.GetRefundRequest(id);
    //    if (refundRequest == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(refundRequest);
    //}
    //88a76c08-8d7e-4d55-8c1f-597e4b61c125
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRefundRequest(Guid id, UpdateRefundRequestModel refundRequestInput)
    {
        return NotFound();
    }



    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateRefundRequest(Guid id, string status)
    {
        try
        {
            RefundRequestViewModel refundRequest = await _refundRequestService.GetRefundRequest(id);
            if (refundRequest == null)
            {
                return BadRequest();
            }

            refundRequest.Status = status;

            // Chuyển đổi từ RefundRequestViewModel sang UpdateRefundRequestModel
            UpdateRefundRequestModel model = new UpdateRefundRequestModel
            {
                TransactionId = refundRequest.TransactionId,
                RefundRequestDate = refundRequest.RefundRequestDate,
                Description = refundRequest.Description,
                Reason = refundRequest.Reason,
                Status = refundRequest.Status
            };

            // Gọi phương thức cập nhật từ dịch vụ
            await _refundRequestService.UpdateRefundRequest(id, model);

            // Trả về kết quả
            return Ok(refundRequest);
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