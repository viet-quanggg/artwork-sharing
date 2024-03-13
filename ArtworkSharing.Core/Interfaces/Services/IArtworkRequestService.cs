using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IArtworkRequestService
{
    Task<IList<ArtworkRequestViewModel>> GetArtworkServices(int pageNumber, int pageSize);
    Task<ArtworkRequestViewModel> GetArtworkService(Guid guid);
    Task<UpdateArtworkRequestModel> UpdateArtworkRequest(Guid id, UpdateArtworkRequestModel uam);
    Task<bool> DeleteArtworkRequest(Guid id);
    Task CreateArtworkRequest(ArtworkService artworkService);
}