using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.User;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNewUser(User user)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var repo = _unitOfWork.UserRepository;
                await repo.AddAsync(user);

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                throw new Exception();
            }
        }

        public async Task DeleteUser(Guid userId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var repo = _unitOfWork.UserRepository;
                var user = await repo.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    await repo.DeleteAsync(user);
                    await _unitOfWork.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw new Exception();
            }
        }

        public async Task<UserViewModel> GetUser(Guid userId)
            => AutoMapperConfiguration.Mapper.Map<UserViewModel>(
                await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Id == userId));

        public async Task<IList<UserViewModel>> GetUsers(int pageNumber, int pageSize)
        {
            try
            {
                int itemsToSkip = (pageNumber - 1) * pageSize;

                var list = await _unitOfWork.UserRepository
                    .Include(u => u.Transactions)
                    .Include(u => u.UserRoles)
                    .Include(u => u.ArtworkServices)
                    .Skip(itemsToSkip)
                    .Take(pageSize)
                    .ToListAsync();

                return AutoMapperConfiguration.Mapper.Map<IList<UserViewModel>>(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<UserViewModel> UpdateUser(Guid userId, UpdateUserModelAdmin user)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var repo = _unitOfWork.UserRepository;
                var Uuser = await repo.FirstOrDefaultAsync(u => u.Id == userId);
                if (Uuser == null)
                {
                    throw new KeyNotFoundException();
                }
                else
                {

                    Uuser.Name = user.Name;
                    Uuser.Gender = user.Gender;
                    Uuser.Status = user.Status;
                    Uuser.PhotoUrl = user.PhotoUrl;
                    repo.UpdateUser(Uuser);

                    await _unitOfWork.CommitTransaction();
                    await _unitOfWork.SaveChangesAsync();
                    return await GetUser(userId);
                }

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}

