using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IArtworkRequestService
{
    Task<List<ArtworkRequestViewModel>> GetArtworkServices(int pageNumber, int pageSize);
    Task<ArtworkRequestViewModel> GetArtworkService(Guid guid);
    
    //User Functions
    Task<List<ArtworkRequestViewModelUser>> GetArtworkRequestsByUser(Guid userId);
    Task<bool> CancelArtworkRequestByUser(Guid requestId);
    //User Functions

    //Artist Functions
    Task<List<ArtworkRequestViewModelUser>> GetArtworkRequestByArtist(Guid artistId);
    Task<bool> CancelArtworkRequestByArtist(Guid requestId);
    Task<bool> AcceptArtworkRequestByArtist(Guid requestId);

    //Artist Functions
    
    Task<UpdateArtworkRequestModel> UpdateArtworkRequest(Guid id, UpdateArtworkRequestModel uam);
    Task<bool> DeleteArtworkRequest(Guid id);
    Task<Core.Domain.Entities.ArtworkService> CreateArtworkRequest(CreateArtworkRequestModel carm);

}