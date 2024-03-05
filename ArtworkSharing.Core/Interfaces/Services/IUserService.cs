using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Users;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IList<User>> GetUsers();
        Task<User> GetUser(Guid userId);
        Task CreateNewUser(User user);
        Task UpdateUser(User user);

        Task<bool> DeleteUser(Guid userId);
        Task<UserViewModel> UpdateUser(UpdateUserModel userModel);
        Task<UserViewModel> CreateUser(CreateUserModel createUser);
        Task<UserViewModel> GetOne(Guid id);
    }
}

