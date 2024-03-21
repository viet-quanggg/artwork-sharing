using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.DAL;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services;

public class ArtworkRequestService : IArtworkRequestService
{
    private readonly UnitOfWork _unitOfWork;

    public ArtworkRequestService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ArtworkRequestViewModel>> GetArtworkServices(int pageNumber, int pageSize)
    {
        try
        {
            var itemsToSkip = (pageNumber - 1) * pageSize;

            var list = await _unitOfWork.ArtworkServiceRepository
                .Include(a => a.Transactions)
                .Skip(itemsToSkip)
                .Take(pageSize)
                .ToListAsync();

            return AutoMapperConfiguration.Mapper.Map<List<ArtworkRequestViewModel>>(list);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public async Task<ArtworkRequestViewModel> GetArtworkService(Guid guid)
    {
        return AutoMapperConfiguration.Mapper.Map<ArtworkRequestViewModel>(
            await _unitOfWork.ArtworkServiceRepository.FirstOrDefaultAsync(a => a.Id == guid));
    }

    public async Task<UpdateArtworkRequestModel> UpdateArtworkRequest(Guid id, UpdateArtworkRequestModel uam)
    {
        var artworkService = await _unitOfWork.ArtworkServiceRepository.FirstOrDefaultAsync(_ => _.Id == id);
        if (artworkService == null) return null;

        return null;
    }

    public async Task<bool> DeleteArtworkRequest(Guid id)
    {
        var artworkService = await _unitOfWork.ArtworkServiceRepository.FirstOrDefaultAsync(_ => _.Id == id);

        if (artworkService == null) throw new ArgumentNullException(nameof(artworkService));

        await _unitOfWork.ArtworkServiceRepository.DeleteAsync(artworkService);

        var rs = await _unitOfWork.SaveChangesAsync();

        return rs > 0;
    }

    public async Task<Core.Domain.Entities.ArtworkService> CreateArtworkRequest(CreateArtworkRequestModel carm)
    {
        if (carm != null)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                var ArtworkServicerepo = _unitOfWork.ArtworkServiceRepository;

                var artworkRequest = AutoMapperConfiguration.Mapper.Map<Core.Domain.Entities.ArtworkService>(carm);

                artworkRequest.RequestedDate = DateTime.Now;
                artworkRequest.Status = ArtworkServiceStatus.Pending;
                var depositAmount = carm.RequestedPrice * (carm.RequestedDeposit / 100);

                artworkRequest.RequestedDeposit = depositAmount;

                await ArtworkServicerepo.AddAsync(artworkRequest);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
                return artworkRequest;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return null;
    }
}