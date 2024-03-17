using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.User;
using ArtworkSharing.Core.ViewModels.Users;
using UserViewModel = ArtworkSharing.Core.ViewModels.User.UserViewModel;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IUserService
{
    Task<IList<UserViewModel>> GetUsers(int pageNumber, int pageSize);
    Task<UserViewModel> GetUserAdmin(Guid userId);
    Task CreateNewUser(User user);
    Task<UserViewModel> UpdateUser(Guid userId, UpdateUserModelAdmin user);
    Task DeleteUserAdmin(Guid userId);

    Task UpdateUser(User user);

    Task<bool> DeleteUser(Guid userId);
    Task<ViewModels.Users.UserViewModel> UpdateUser(UpdateUserModel userModel);
    Task<ViewModels.Users.UserViewModel> CreateUser(CreateUserModel createUser);
    Task<ViewModels.Users.UserViewModel> GetOne(Guid id);
}