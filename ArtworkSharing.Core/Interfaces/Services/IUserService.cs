using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.User;
using ArtworkSharing.Core.ViewModels.Users;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IList<ViewModels.User.UserViewModel>> GetUsers(int pageNumber, int pageSize);
        Task<ViewModels.User.UserViewModel> GetUserAdmin(Guid userId);
        Task CreateNewUser(User user);
        Task<ViewModels.User.UserViewModel> UpdateUser(Guid userId, UpdateUserModelAdmin user);
        Task DeleteUserAdmin(Guid userId);

        Task UpdateUser(User user);

        Task<bool> DeleteUser(Guid userId);
        Task<ViewModels.Users.UserViewModel> UpdateUser(UpdateUserModel userModel);
        Task<ViewModels.Users.UserViewModel> CreateUser(CreateUserModel createUser);
        Task<ViewModels.Users.UserViewModel> GetOne(Guid id);
    }
}

