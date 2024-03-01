using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IList<User>> GetUsers();
        Task<User> GetUser(Guid userId);
        Task CreateNewUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(Guid userId);
       
    }
}

