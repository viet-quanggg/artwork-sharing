using System.Linq.Expressions;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Package;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services;

public class PackageService : IPackageService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IUserRoleService _userRoleService;

    private readonly IArtistService _artistService;

    private readonly IArtistPackageService _artistPackageService;

    private readonly IPackageService _packageService;

    private readonly UserManager<Core.Domain.Entities.User> _userManager;
    public PackageService(IUnitOfWork unitOfWork, UserManager<Core.Domain.Entities.User> userManager
        , IUserRoleService userRoleService, IArtistService artistService, IArtistPackageService artistPackageService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userRoleService = userRoleService;
        _artistService = artistService;
        _artistPackageService = artistPackageService;
    }

    public async Task<IList<PackageViewModel>> GetAll()
    {
        var queryableData = _unitOfWork.PackageRepository.GetAll().AsQueryable();

        return AutoMapperConfiguration.Mapper.Map<IList<PackageViewModel>>(
            await queryableData.ToListAsync(CancellationToken.None));
    }

    public async Task<PackageViewModel> GetOne(Guid PackageId)
    {
        return AutoMapperConfiguration.Mapper.Map<PackageViewModel>(
            await _unitOfWork.PackageRepository.FirstOrDefaultAsync(x => x.Id == PackageId));
    }

    public async Task Update(Package PackageInput)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var PackageRepos = _unitOfWork.PackageRepository;
            var Package = await PackageRepos.GetAsync(mc => mc.Id == PackageInput.Id);
            if (Package == null) throw new KeyNotFoundException();


            Package.Name = Package.Name;

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task Add(Package PackageInput)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var PackageRepos = _unitOfWork.PackageRepository;
            PackageRepos.Add(PackageInput);

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task Delete(Guid PackageId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var PackageRepository = _unitOfWork.PackageRepository;

            var Package = await PackageRepository.GetAsync(mc => mc.Id == PackageId);

            if (Package == null)
                throw new KeyNotFoundException();

            PackageRepository.DeleteAsync(Package);

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public IEnumerable<Package> Get(Expression<Func<Package, bool>> filter = null,
        Func<IQueryable<Package>, IOrderedQueryable<Package>> orderBy = null, string includeProperties = "",
        int? pageIndex = null, int? pageSize = null)
    {
        try
        {
            var PackageRepository =
                _unitOfWork.PackageRepository.Get(filter, orderBy, includeProperties, pageIndex, pageSize);

            return PackageRepository;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task CheckOutPackage(TransactionViewModel transaction)
    {
      
        try
        {
            if (transaction != null && transaction.PackageId.HasValue)
            {
                await _unitOfWork.BeginTransaction();
                Guid k = Guid.Parse(transaction.PackageId.Value.ToString());
                //  Guid currentUserId = new Guid(transaction.PackageId.Value);
                //  PackageViewModel package = await _packageService.GetOne((Guid)transaction.PackageId);
                Package package =  _unitOfWork.PackageRepository.Get(x => x.Id == transaction.PackageId);

                //UserRole userRole = new UserRole
                //{
                //    UserId = transaction.AudienceId,
                //    RoleId = Guid.Parse("820fa473-2833-4dd6-9d04-eb8019bfbad5")  // guid for idRole

                //};
                //await _userRoleService.UpdateRole(userRole);
               
               
                var user = await _userManager.FindByIdAsync(transaction.AudienceId.ToString());
                var roleResult = await _userManager.AddToRoleAsync(user, RoleOfSystem.Artist.ToString());
                //Create AritrstPackage
                // Create Banking for Aritst
                Artist artist = new Artist
                {
                    Id = new Guid(),
                    UserId = transaction.AudienceId,
                    Bio = string.Empty  ,
                     BankAccount = string.Empty,
                };

                ArtistPackage artistPackage = new ArtistPackage
                {
                    Id = new Guid(),
                PackageId = transaction.PackageId ?? new Guid(),
                TransactionId = transaction.Id,
                PurchasedDate = DateTime.UtcNow.AddDays(package.Duration),
                Artist = artist


                };

                await _unitOfWork.ArtworkPackageRepository.AddAsync(artistPackage);
                await _unitOfWork.CommitTransaction();
            }
            else
            {
                // Handle the case where transaction is null or PackageId is null
            }
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }
}