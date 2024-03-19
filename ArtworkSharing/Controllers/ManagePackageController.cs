using System.Linq.Expressions;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Package;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Service.Services;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[Route("[controller]")]
[ApiController]
public class ManagePackageController : Controller
{
    private readonly IPackageService _packageService;

    private readonly ITransactionService _transactionService;

    private readonly IUserRoleService _userRoleService;

    private readonly IArtistPackageService _artistPackageService;

    private readonly IArtistService _artistService;

    private readonly HttpClient _httpClient;

    public ManagePackageController(IPackageService packageService, ITransactionService transactionService, IUserRoleService userRoleService, IArtistPackageService artistPackageService, IArtistService artistService)
    {
        _packageService = packageService;
        _transactionService = transactionService;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7270/api/");
        _userRoleService = userRoleService;
        _artistPackageService = artistPackageService;
        _artistService = artistService;
    }

    //[HttpGet]
    //public async Task<ActionResult<List<PackageViewModel>>> GetAllPackages()
    //{
    //    var packages = await _packageService.GetAll();
    //    return Ok(packages);
    //}

    [HttpGet(Name = "GetPackageWithPaging")]
    public async Task<ActionResult<List<PackageViewModel>>> GetPackageWithPaging(
        [FromQuery] int? pageIndex = null,
        [FromQuery] int? pageSize = null)
    {
        try
        {
            Expression<Func<Package, bool>> filter = null;
            Func<IQueryable<Package>, IOrderedQueryable<Package>> orderBy = null;
            var includeProperties = "";

            var packages = _packageService.Get(filter, orderBy, includeProperties, pageIndex, pageSize);
            // Convert to view model if needed
            // var packageViewModels = packages.Select(p => new PackageViewModel { ... }).ToList();
            return Ok(packages);
        }
        catch (Exception)
        {
            return StatusCode(500); // Internal Server Error
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PackageViewModel>> GetPackage(Guid id)
    {
        var package = await _packageService.GetOne(id);
        if (package == null) return NotFound();
        return Ok(package);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePackage(Guid id, Package packageInput)
    {
        try
        {
            await _packageService.Update(packageInput);
            return Ok(packageInput);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePackage(Package packageInput)
    {
        try
        {
            await _packageService.Add(packageInput);
            return CreatedAtAction(nameof(GetPackage), new { id = packageInput.Id }, packageInput);
        }
        catch (Exception)
        {
            return StatusCode(500); // Internal Server Error
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackage(Guid id)
    {
        try
        {
            await _packageService.Delete(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    //9c68f75b-ed05-4718-b6b8-05211342a80f
    //8f5c103a-8cfb-4cc2-bf93-05286b79c43e
    //098901890883
    [HttpPut("{UserId}/checkout")]
    public async Task<IActionResult> CheckOutPackage(Guid UserId,Guid PackageId)
    {
        try
        {
            PackageViewModel package = await _packageService.GetOne(PackageId);
            if (package == null) return StatusCode(404);    
            //Create transaction
            Transaction transaction = new Transaction
            {
                Id = new Guid(),
                ArtworkId = null,
                ArtworkServiceId = null,
                AudienceId = UserId,
                TotalBill = package.Price,
                CreatedDate = DateTime.UtcNow,
                Status = TransactionStatus.Fail, // Đặt trạng thái mặc định
                Type = TransactionType.Package,
                PackageId = PackageId,
                
            };
            await _transactionService.AddTransaction(transaction);

            // Call API VNPay
            var response = await _httpClient.GetAsync($"Payment/{transaction.Id}");

            //if (response.IsSuccessStatusCode)
            //{
            //    //Update Transaction
            //    var result = await response.Content.ReadAsStringAsync();
            //    transaction.Status = TransactionStatus.Success;
            //    UpdateTransactionModel updateTransactionModel = new UpdateTransactionModel {
            //        Id = new Guid(),
            //        ArtworkId = null,
            //        ArtworkServiceId = null,
            //        AudienceId = UserId,
            //        TotalBill = package.Price,
            //        CreatedDate = DateTime.UtcNow,
            //        Status = TransactionStatus.Success, // Đặt trạng thái mặc định
            //        Type = TransactionType.Package,

            //    };
            //    await _transactionService.UpdateTransaction(transaction.Id,updateTransactionModel);

            //    //Update Role
            //    UserRole userRole = new UserRole {
            //          UserId = transaction.AudienceId,
            //          RoleId = new Guid(), // guid for idRole
            //    };
            //    await _userRoleService.UpdateRole(userRole);

            //    //Create AritrstPackage
            //    ArtistPackage artistPackage = new ArtistPackage { 
            //     Id = new Guid(),
            //     ArtistId = transaction.AudienceId,
            //     PackageId = transaction.PackageId ?? new Guid(),
            //     TransactionId= transaction.Id,
            //     PurchasedDate = DateTime.UtcNow.AddDays(package.Duration),


            //    };
            //    // Create Banking for Aritst
            //    Artist artist = new Artist { 
            //        Id = transaction.AudienceId,
            //        UserId = transaction.AudienceId,
            //        BankAccount = BankingAccount
            //    };
            //    await _artistService.Add(artist);
            //    _artistPackageService.Add(artistPackage);
            //    return Ok(result);
            //}
            //else
            //{
            //    // Xử lý lỗi
            //    return StatusCode((int)response.StatusCode);
            //}

            return Ok(response);
           
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{UserId}/successcheckout")]
    public async Task<IActionResult> SuccessfullPackage(Guid UserId, Guid PackageId, string BankingAccount,Guid TransactionId)
    {
        try
        {
            PackageViewModel package = await _packageService.GetOne(PackageId);
            if (package == null) return StatusCode(404);
            //Create transaction
            Transaction transaction = await _transactionService.GetOne(TransactionId);
            if (transaction == null) return StatusCode(404);
         
           

            
             
                transaction.Status = TransactionStatus.Success;
                UpdateTransactionModel updateTransactionModel = new UpdateTransactionModel
                {
                    Id = new Guid(),
                    ArtworkId = null,
                    ArtworkServiceId = null,
                    AudienceId = UserId,
                    TotalBill = package.Price,
                    CreatedDate = DateTime.UtcNow,
                    Status = TransactionStatus.Success, // Đặt trạng thái mặc định
                    Type = TransactionType.Package,

                };
                await _transactionService.UpdateTransaction(transaction.Id, updateTransactionModel);

                //Update Role
                UserRole userRole = new UserRole
                {
                    UserId = transaction.AudienceId,
                    RoleId = new Guid(), // guid for idRole
                };
                await _userRoleService.UpdateRole(userRole);

                //Create AritrstPackage
                ArtistPackage artistPackage = new ArtistPackage
                {
                    Id = new Guid(),
                    ArtistId = transaction.AudienceId,
                    PackageId = transaction.PackageId ?? new Guid(),
                    TransactionId = transaction.Id,
                    PurchasedDate = DateTime.UtcNow.AddDays(package.Duration),


                };
                // Create Banking for Aritst
                Artist artist = new Artist
                {
                    Id = transaction.AudienceId,
                    UserId = transaction.AudienceId,
                    BankAccount = BankingAccount
                };
                await _artistService.Add(artist);
                _artistPackageService.Add(artistPackage);
                return Ok();
          

        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}