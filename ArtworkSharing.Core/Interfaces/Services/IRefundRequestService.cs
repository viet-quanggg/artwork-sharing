using System.Linq.Expressions;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.RefundRequests;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IRefundRequestService
{
    Task<List<RefundRequestViewModel>> GetAll();

    Task<RefundRequestViewModel> GetRefundRequest(Guid id);

    Task<RefundRequestViewModel> UpdateRefundRequest(Guid id, UpdateRefundRequestModel urm);

    Task<bool> DeleteRefundRequest(Guid id);

    Task CreateRefundRequest(CreateRefundRequestModel crrm);

    Task<List<RefundRequestViewModelUser>> GetRefundRequestForUser(Guid userId);
    Task<RefundRequestViewModelUser> GetRefundRequestDetail(Guid refundId);
    Task<bool> CancelRefundRequestByUser(Guid refundId);


    IEnumerable<RefundRequest> Get(
    Expression<Func<RefundRequest, bool>> filter = null,
    Func<IQueryable<RefundRequest>, IOrderedQueryable<RefundRequest>> orderBy = null,
    string includeProperties = "",
    int? pageIndex = null,
    int? pageSize = null
    );

    Task<int> Count(Expression<Func<RefundRequest, bool>> filter = null);

}

