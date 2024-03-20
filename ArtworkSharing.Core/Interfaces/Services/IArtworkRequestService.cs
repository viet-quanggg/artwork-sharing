using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IArtworkRequestService
{
    Task<List<ArtworkRequestViewModel>> GetArtworkServices(int pageNumber, int pageSize);
    Task<ArtworkRequestViewModel> GetArtworkService(Guid guid);
    Task<List<ArtworkRequestViewModelUser>> GetArtworkRequestsByUser(Guid userId);
    Task<UpdateArtworkRequestModel> UpdateArtworkRequest(Guid id, UpdateArtworkRequestModel uam);
    Task<bool> DeleteArtworkRequest(Guid id);
    Task<Core.Domain.Entities.ArtworkService> CreateArtworkRequest(CreateArtworkRequestModel carm);
}