using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtworkSharing.Service.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateRole(UserRole role)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var repo = _unitOfWork.UserRoleRepository;
                await repo.AddAsync(role);
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw new Exception("Failed to create role.", ex);
            }
        }

        public async Task DeleteRole(Guid roleId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var repo = _unitOfWork.UserRoleRepository;
                var role = await repo.FirstOrDefaultAsync(r => r.RoleId == roleId);
                if (role == null)
                    throw new KeyNotFoundException("Role not found.");

                await repo.DeleteAsync(role);
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw new Exception("Failed to delete role.", ex);
            }
        }

        public async Task<IList<UserRole>> GetRoles()
        {
            return await _unitOfWork.UserRoleRepository.GetAllAsync();
        }

        public async Task<UserRole> GetRole(Guid roleId)
        {
            return await _unitOfWork.UserRoleRepository.FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        public async Task UpdateRole(UserRole role)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var repo = _unitOfWork.UserRoleRepository;
                var existingRole = await repo.FirstOrDefaultAsync(r => r.UserId == role.UserId);
                if (existingRole == null)
                    throw new KeyNotFoundException("Role not found.");

                // Update role properties
                existingRole.RoleId = role.RoleId; // For example

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw new Exception("Failed to update role.", ex);
            }
        }
    }
}
