using System;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL.Extensions;

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

        public async Task<User> GetUser(Guid userId)
        {
            return await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public Task<IList<User>> GetUsers()
        {
            return _unitOfWork.UserRepository.GetAllAsync();
        }

        public async Task UpdateUser(User user)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var repo = _unitOfWork.UserRepository;
                var Uuser = await repo.FirstOrDefaultAsync(u => u.Id == user.Id);
                if (Uuser == null)
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    Uuser = user;


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

