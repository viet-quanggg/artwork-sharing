using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Helpers;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Users;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using AutoMapper;

namespace ArtworkSharing.Service.Services
{
    public class UserService : IUserService
    {
        // Temp
        int pageSize, pageIndex;

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
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

        public async Task<UserViewModel> CreateUser(CreateUserModel createUser)
        {
            var repo = _unitOfWork.UserRepository;
            var u = AutoMapperConfiguration.Mapper.Map<User>(createUser);

            u.Id = Guid.NewGuid();
            u.Status = true;

            await repo.AddAsync(u);
            await _unitOfWork.SaveChangesAsync();
            return await GetOne(u.Id);
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var repo = _unitOfWork.UserRepository;
            var user = await repo.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                await repo.DeleteAsync(user);
            }
            return (await _unitOfWork.SaveChangesAsync()) > 0;
        }

        public async Task<UserViewModel> GetOne(Guid id)
            => AutoMapperConfiguration.Mapper.Map<UserViewModel>(await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Id == id && u.Status));

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

        public async Task<UserViewModel> UpdateUser(UpdateUserModel um)
        {
            var repo = _unitOfWork.UserRepository;
            var u = await repo.FirstOrDefaultAsync(u => u.Id == um.Id);
            if (u == null)
            {
                throw new KeyNotFoundException();
            }

            u.Name = um.Name ?? u.Name;
            u.Email = um.Email ?? u.Email;
            _unitOfWork.UserRepository.UpdateUser(u);

            await _unitOfWork.SaveChangesAsync();
            return await GetOne(um.Id);
        }


    }
}

