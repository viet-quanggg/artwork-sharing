using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;

namespace ArtworkSharing.Service.Services;

public class ArtistPackageService : IArtistPackageService
{
    private readonly IUnitOfWork _unitOfWork;

    public ArtistPackageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Add(ArtistPackage artistPackage)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var repos = _unitOfWork.ArtworkPackageRepository;
            await repos.AddAsync(artistPackage);

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task Delete(Guid artistPackageId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var repos = _unitOfWork.ArtworkPackageRepository;
            var ap = await repos.FindAsync(artistPackageId);
            if (ap == null)
                throw new KeyNotFoundException();

            await repos.DeleteAsync(ap);

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task<IList<ArtistPackage>> GetAll()
    {
        return await _unitOfWork.ArtworkPackageRepository.GetAllAsync();
    }

    public async Task<ArtistPackage> GetOne(Guid artistPackageId)
    {
        return await _unitOfWork.ArtworkPackageRepository.FindAsync(artistPackageId);
    }

    public async Task Update(ArtistPackage artistPackage)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var repos = _unitOfWork.ArtworkPackageRepository;
            var ap = await repos.FindAsync(artistPackage.Id);
            if (ap == null)
                throw new KeyNotFoundException();

            //a.Name = a.Name;

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }
}