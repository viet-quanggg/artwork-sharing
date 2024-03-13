using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Repositories;

public interface IRefundRequestRepository : IRepository<RefundRequest>
{
    void UpdateRefundRequest(RefundRequest refundRequest);
}