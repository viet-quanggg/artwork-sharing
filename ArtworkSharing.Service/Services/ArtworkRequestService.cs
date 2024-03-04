using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.DAL;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services;

public class ArtworkRequestService : IArtworkRequestService
{
    private UnitOfWork _unitOfWork;

    public ArtworkRequestService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IList<ArtworkRequestViewModel>> GetArtworkServices() =>
        AutoMapperConfiguration.Mapper.Map<List<ArtworkRequestViewModel>>(await _unitOfWork.ArtworkServiceRepository.GetAll().AsQueryable().ToListAsync()
            );


    public async Task<ArtworkRequestViewModel> GetArtworkService(Guid guid) =>
        AutoMapperConfiguration.Mapper.Map<ArtworkRequestViewModel>(
            await _unitOfWork.ArtworkServiceRepository.FirstOrDefaultAsync(a => a.TransactionId == guid));

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

    public async Task CreateArtworkRequest(Core.Domain.Entities.ArtworkService artworkService)
    {
        var service = await _unitOfWork.ArtworkServiceRepository.FirstOrDefaultAsync(_ =>
                _.TransactionId == artworkService.TransactionId);
        if (service == null) throw new ArgumentException(nameof(service));
        service = artworkService;
        // refundRequest = AutoMapperConfiguration.Mapper.Map<RefundRequest>(refund);
        await _unitOfWork.ArtworkServiceRepository.AddAsync(service);
        await _unitOfWork.SaveChangesAsync();
    }
    
    
}