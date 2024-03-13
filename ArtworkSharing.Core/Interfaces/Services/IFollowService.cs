using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Repositories;

public interface IFollowService
{
    Task<IList<Follow>> GetAll();
    Task<Follow> GetOne(Guid followId);
    Task Update(Follow follow);
    Task Add(Follow follow);
    Task Delete(Guid followId);
    Task<bool> IsFollowing(Guid currentUserId, Guid followUserId);
    Task FollowUser(Guid currentUserId, Guid followUserId);
    Task UnFollowUser(Guid currentUserId, Guid followUserId);
}