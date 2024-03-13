using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    void UpdateUser(User u);
}