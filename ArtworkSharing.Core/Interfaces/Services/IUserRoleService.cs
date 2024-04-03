using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IUserRoleService
    {
        Task<IList<UserRole>> GetRoles();
        Task<UserRole> GetRole(Guid roleId);
        Task UpdateRole(UserRole role);
        Task CreateRole(UserRole role);
        Task DeleteRole(Guid roleId);
    }
}
