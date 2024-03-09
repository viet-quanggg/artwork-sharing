using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.User;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IList<UserViewModel>> GetUsers(int pageNumber, int pageSize);
        Task<UserViewModel> GetUser(Guid userId);
        Task CreateNewUser(User user);
        Task<UserViewModel> UpdateUser(Guid userId, UpdateUserModelAdmin user);
        Task DeleteUser(Guid userId);
       
    }
}

