using ArtworkSharing.Core.ViewModels.RefundRequests;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IRefundRequestService
{
    Task<List<RefundRequestViewModel>> GetAll();

    Task<RefundRequestViewModel> GetRefundRequest(Guid id);

    Task<RefundRequestViewModel> UpdateRefundRequest(Guid id, UpdateRefundRequestModel urm);

    Task<bool> DeleteRefundRequest(Guid id);

    Task CreateRefundRequest(CreateRefundRequestModel crrm);
}