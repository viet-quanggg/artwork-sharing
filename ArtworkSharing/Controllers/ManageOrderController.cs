using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.RefundRequests;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ArtworkSharing.Controllers;

[Route("[controller]")]
[ApiController]
public class ManageOrderController : Controller
{
    private readonly ITransactionService _transactionService;
    private readonly IPackageService _packageService;
    private readonly IArtistService _artistService;
    private readonly IArtworkService _artworkService;
    private readonly IArtworkRequestService _artworkRequestService;

    public ManageOrderController(ITransactionService transactionService, IPackageService packageService, IArtistService artistService, IArtworkService artworkService, IArtworkRequestService artworkRequestService)
    {
        _transactionService = transactionService;
        _packageService = packageService;
        _artistService = artistService;
        _artworkService = artworkService;
        _artworkRequestService = artworkRequestService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionViewModel>>> GetAllTransactions()
    {
        var transactions = await _transactionService.GetAll();
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionViewModel>> GetTransaction(Guid id)
    {
        var transaction = await _transactionService.GetTransaction(id);
        if (transaction == null) return NotFound();
        return Ok(transaction);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetTransactionCount([FromQuery] string searchKeyword = null)
    {
        try
        {
            Expression<Func<Transaction, bool>> filter = null;

            // Tạo điều kiện tìm kiếm dựa trên searchKeyword
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter = t => t.CreatedDate.ToString().Contains(searchKeyword)
                           || t.TotalBill.ToString().Contains(searchKeyword);
            }
            int count = await _transactionService.Count(filter);
            return Ok(count);
        }
        catch (Exception)
        {
            return StatusCode(500); // Lỗi máy chủ nội bộ
        }
    }

    [HttpGet("countArtist")]
    public async Task<ActionResult<int>> GetTransactionCountArtist(Guid ArtistId,[FromQuery] string searchKeyword = null)
    {
        try
        {
            Expression<Func<Transaction, bool>> filter = r => ((r.Artwork.ArtistId == ArtistId) || (r.ArtworkService.ArtistId == ArtistId));

            // Tạo điều kiện tìm kiếm dựa trên searchKeyword
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter = t => t.CreatedDate.ToString().Contains(searchKeyword)
                           || t.TotalBill.ToString().Contains(searchKeyword);
            }
            int count = await _transactionService.Count(filter);
            return Ok(count);
        }
        catch (Exception)
        {
            return StatusCode(500); // Lỗi máy chủ nội bộ
        }
    }




    [HttpGet("GeTransactionWithPaging")]
    public async Task<ActionResult<List<Transaction>>> GetPackageWithPaging(
 [FromQuery] int? pageIndex = null,
 [FromQuery] int? pageSize = null, [FromQuery] string searchKeyword = null)
    {
        try
        {
            Expression<Func<Transaction, bool>> filter = null;

            // Tạo điều kiện tìm kiếm dựa trên searchKeyword
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter = t => t.CreatedDate.ToString().Contains(searchKeyword)
                           || t.TotalBill.ToString().Contains(searchKeyword);
            }

            // Khởi tạo hàm sắp xếp giảm dần theo thời gian
            Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy = q => q.OrderByDescending(p => p.CreatedDate);

            string includeProperties = "Package,Artwork,ArtworkService,Audience";

            var transactions = _transactionService.Get(filter, orderBy, includeProperties, pageIndex, pageSize);
           
            // Chuyển đổi sang view model nếu cần
            // var packageViewModels = packages.Select(p => new PackageViewModel { ... }).ToList();
            return Ok(transactions);
        }
        catch (Exception)
        {
            return StatusCode(500); // Lỗi máy chủ nội bộ
        }
    }

    [HttpGet("GeTransactionWithPagingArtist")]
    public async Task<ActionResult<List<Transaction>>> GetTransactionWithPaginArtist([FromQuery] Guid ArtistId,
[FromQuery] int? pageIndex = null,
[FromQuery] int? pageSize = null, [FromQuery] string searchKeyword = null)
    {
        try
        {
            Expression<Func<Transaction, bool>> filter = r => ((r.Artwork.ArtistId==ArtistId) || (r.ArtworkService.ArtistId == ArtistId));

            // Tạo điều kiện tìm kiếm dựa trên searchKeyword
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter = t => t.CreatedDate.ToString().Contains(searchKeyword)
                           || t.TotalBill.ToString().Contains(searchKeyword);
            }

            // Khởi tạo hàm sắp xếp giảm dần theo thời gian
            Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy = q => q.OrderByDescending(p => p.CreatedDate);

            string includeProperties = "Package,Artwork,ArtworkService,Audience";

            var transactions = _transactionService.Get(filter, orderBy, includeProperties, pageIndex, pageSize);

            // Chuyển đổi sang view model nếu cần
            // var packageViewModels = packages.Select(p => new PackageViewModel { ... }).ToList();
            return Ok(transactions);
        }
        catch (Exception)
        {
            return StatusCode(500); // Lỗi máy chủ nội bộ
        }
    }

    //[HttpPost]
    //public async Task<ActionResult<TransactionViewModel>> CreateTransaction(CreateTransactionModel ctm)
    //{
    //	// Validate ctm if needed
    //	var createdTransaction = await _transactionService.CreateTransaction(ctm);
    //	return CreatedAtAction(nameof(GetTransaction), new { id = createdTransaction.Id }, createdTransaction);
    //}

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(Guid id, UpdateTransactionModel utm)
    {
        try
        {
            var updatedTransaction = await _transactionService.UpdateTransaction(id, utm);
            return Ok(updatedTransaction);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        var result = await _transactionService.DeleteTransaction(id);
        if (result) return NoContent();
        return NotFound();
    }
}