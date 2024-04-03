using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.Core.ViewModels.Transactions;
using Microsoft.AspNetCore.Http;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IArtworkRequestService
{
    Task<List<ArtworkRequestViewModel>> GetArtworkServices(int pageNumber, int pageSize);
    Task<ArtworkRequestViewModel> GetArtworkService(Guid guid);
    
    //User Functions
    Task<List<ArtworkRequestViewModelUser>> GetArtworkRequestsByUser(Guid userId);
    Task<bool> CancelArtworkRequestByUser(Guid requestId);
    Task<bool> ChangeStatusAfterDeposit(TransactionViewModel tvm);
    //User Functions

    //Artist Functions
    Task<List<ArtworkRequestViewModelUser>> GetArtworkRequestByArtist(Guid artistId);
    Task<bool> CancelArtworkRequestByArtist(Guid requestId);
    Task<bool> AcceptArtworkRequestByArtist(Guid requestId);

    Task<bool> CommitArtworkRequest(Guid id, CommitArtworkRequestModel uam);
    //Artist Functions

    /*    Task<UpdateArtworkRequestModel> UpdateArtworkRequest(Guid id, UpdateArtworkRequestModel uam);
    */
      Task<bool> DeleteArtworkRequest(Guid id);
    Task<Core.Domain.Entities.ArtworkService> CreateArtworkRequest(CreateArtworkRequestModel carm);

}