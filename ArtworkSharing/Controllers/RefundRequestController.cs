using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.RefundRequests;
using ArtworkSharing.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using ArtworkSharing.Core.Domain.Enums;

namespace ArtworkSharing.Controllers;

[Route("[controller]")]
[ApiController]
public class RefundRequestController : ControllerBase
{
    private readonly IRefundRequestService _refundRequestService;
    
    private readonly ITransactionService _transactionService;

    private readonly IArtworkService _artworkService;

    private readonly IArtistService _artistService;


    [HttpPost("createRefundRequestUser")]
    public async Task<ActionResult> CreateRefundRequestUser(CreateRefundRequestModel crrm)
    {
        try
        {
            await _refundRequestService.CreateRefundRequest(crrm);
            return Ok(crrm);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    

 

    public RefundRequestController(IRefundRequestService refundRequestService, ITransactionService transactionService, IArtworkService artworkService,IArtistService artistService)
    {
        _refundRequestService = refundRequestService;
        _artworkService = artworkService;
        _transactionService = transactionService;  
        _artistService = artistService;
    }

  

    //[HttpGet("{id}")]
    //public async Task<ActionResult<RefundRequestViewModel>> GetRefundRequest(Guid id)
    //{
    //    var refundRequest = await _refundRequestService.GetRefundRequest(id);
    //    if (refundRequest == null) return NotFound();
    //    return Ok(refundRequest);
    //}


    //[HttpGet]
    //public async Task<ActionResult<List<RefundRequestViewModel>>> GetAllRefundRequests()
    //{
    //    var refundRequests = await _refundRequestService.GetAll();
    //    return Ok(refundRequests);
    //}

    // 5990f7bd-5ee5-4c52-9cce-2c57d90c34ec id aritst
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetRefundRequestCount()
    {
        try
        {
            Expression<Func<RefundRequest, bool>> filter = r => ( (r.Status.Equals(RefundRequestStatus.Pending.ToString())));
            int count = await _refundRequestService.Count(filter);
            return Ok(count);
        }
        catch (Exception)
        {
            return StatusCode(500); // Lỗi máy chủ nội bộ
        }
    }

    [Authorize]
    [HttpGet("countAritst")]
    public async Task<ActionResult<int>> GetRefundRequestCountArist()
    {
        try
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Guid currentUserId = new Guid(userIdClaim?.Value);
            var Artists = await _artistService.GetOneArist(currentUserId);
            if (Artists == null)
            {
                return StatusCode(200);
            }

            Guid ArtistId = Artists.Id;
            Expression<Func<RefundRequest, bool>> filter = r => (r.Transaction.Artwork.ArtistId == ArtistId) && (r.Status.Equals(RefundRequestStatus.AcceptedBySystem.ToString()));
            int count = await _refundRequestService.Count(filter);
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
            // Sử dụng phương thức Join để kết nối các bảng
            Expression<Func<Transaction, bool>> filtert = null;


            // Khởi tạo hàm sắp xếp giảm dần theo thời gian
            Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderByt = q => q.OrderByDescending(p => p.CreatedDate);

            string includePropertiest = "Package,Artwork,ArtworkService,Audience";

            var transactions = _transactionService.Get(filtert, orderByt, includePropertiest, null, null);

            // Lấy TransactionId từ danh sách các Transaction
            List<Guid> listId = new List<Guid> { };
            foreach (var transac in transactions)
            {
                listId.Add(transac.Id);
            }
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

    [HttpGet("GetRefundRequestWithPaging")]
    public async Task<ActionResult<List<RefundRequest>>> GetPackageWithPaging(
 [FromQuery] int? pageIndex = null,
 [FromQuery] int? pageSize = null)
    {
        try
        {

            // Sử dụng phương thức Join để kết nối các bảng
            Expression<Func<Transaction, bool>> filtert =null;


            // Khởi tạo hàm sắp xếp giảm dần theo thời gian
            Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderByt = q => q.OrderByDescending(p => p.CreatedDate);

            string includePropertiest = "Package,Artwork,ArtworkService,Audience";

            var transactions = _transactionService.Get(filtert, orderByt, includePropertiest, null, null);

            // Lấy TransactionId từ danh sách các Transaction
            List<Guid> listId = new List<Guid> { };
            foreach (var transac in transactions)
            {
                listId.Add(transac.Id);
            }

            Expression<Func<RefundRequest, bool>> filter =  r => ( (r.Status.Equals(RefundRequestStatus.Pending.ToString())));

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
    [Authorize]
    [HttpGet("GetRefundRequestWithPagingArist")]
    public async Task<ActionResult<List<RefundRequest>>> GetPackageWithPagingArist( 
[FromQuery] int? pageIndex = null,
[FromQuery] int? pageSize = null)
    {
        try
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Guid currentUserId = new Guid(userIdClaim?.Value);
            var Artists = await _artistService.GetOneArist(currentUserId);
            if (Artists == null)
            {
                return StatusCode(200);
            }

            Guid ArtistId = Artists.Id;
            // Sử dụng phương thức Join để kết nối các bảng
            Expression<Func<Transaction, bool>> filtert = t => t.Artwork.ArtistId == ArtistId;
        

            // Khởi tạo hàm sắp xếp giảm dần theo thời gian
            Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderByt = q => q.OrderByDescending(p => p.CreatedDate);

            string includePropertiest = "Package,Artwork,ArtworkService,Audience";

            var transactions = _transactionService.Get(filtert, orderByt, includePropertiest, null, null);

            // Lấy TransactionId từ danh sách các Transaction
            List<Guid> listId = new List<Guid> {  };
            foreach(var transac in transactions)
            {
                listId.Add(transac.Id);
            }

            Expression<Func<RefundRequest, bool>> filter = r => (listId.Contains(r.TransactionId)) && ( r.Status.Equals(RefundRequestStatus.AcceptedBySystem.ToString())); 
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


    [HttpGet("/RefundRequestByUser/{userId}")]
    public async Task<IActionResult> RefundRequestForUser(Guid userId)
    {
        try
        {
            if (userId != null)
            {
                return Ok(await _refundRequestService.GetRefundRequestForUser(userId));
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    // [Authorize("")]
    [HttpGet("/RefundRequestDetailByUser/{refundId}")]
    public async Task<IActionResult> RefundRequestDetailForUser(Guid refundId)
    {
        try
        {
            if (refundId != null)
            {
                return Ok(await _refundRequestService.GetRefundRequestDetail(refundId));
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPut("/CancelRequestByUser/{refundId}")]
    public async Task<IActionResult> CancelRequestByUser(Guid refundId)
    {
        try
        {
            if (refundId != null)
            {
                return Ok(await _refundRequestService.CancelRefundRequestByUser(refundId));
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
}