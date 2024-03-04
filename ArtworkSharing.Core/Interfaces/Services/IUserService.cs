using System;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.User;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IList<UserViewModel>> GetUsers();
        Task<UserViewModel> GetUser(Guid userId);
        Task CreateNewUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(Guid userId);
    }
}

