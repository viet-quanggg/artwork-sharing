using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.User;
using ArtworkSharing.Core.ViewModels.Users;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;
using UserViewModel = ArtworkSharing.Core.ViewModels.Users.UserViewModel;

namespace ArtworkSharing.Service.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    // Temp
    private int pageSize, pageIndex;

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

    public async Task<UserViewModel> GetOne(Guid id)
    {
        return AutoMapperConfiguration.Mapper.Map<UserViewModel>(
            await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Id == id && u.Status));
    }


    public async Task<bool> DeleteUser(Guid userId)
    {
        var repo = _unitOfWork.UserRepository;
        var user = await repo.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new KeyNotFoundException();
        await repo.DeleteAsync(user);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }


    public async Task<IList<Core.ViewModels.User.UserViewModel>> GetUsers(int pageNumber, int pageSize)
    {
        try
        {
            var itemsToSkip = (pageNumber - 1) * pageSize;

            var list = await _unitOfWork.UserRepository
                .Include(u => u.Transactions)
                .Include(u => u.UserRoles)
                .Include(u => u.ArtworkServices)
                .Skip(itemsToSkip)
                .Take(pageSize)
                .ToListAsync();

            return AutoMapperConfiguration.Mapper.Map<IList<Core.ViewModels.User.UserViewModel>>(list);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public async Task<Core.ViewModels.User.UserViewModel> UpdateUser(Guid userId, UpdateUserModelAdmin user)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var repo = _unitOfWork.UserRepository;
            var Uuser = await repo.FirstOrDefaultAsync(u => u.Id == userId);
            if (Uuser == null) throw new KeyNotFoundException();

            Uuser.Name = user.Name;
            Uuser.Gender = user.Gender;
            Uuser.Status = user.Status;
            Uuser.PhotoUrl = user.PhotoUrl;
            repo.UpdateUser(Uuser);

            await _unitOfWork.CommitTransaction();
            await _unitOfWork.SaveChangesAsync();
            return await GetUserAdmin(userId);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task<Core.ViewModels.User.UserViewModel> GetUserAdmin(Guid userId)
    {
        return AutoMapperConfiguration.Mapper.Map<Core.ViewModels.User.UserViewModel>(
            await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Id == userId));
    }


    public async Task UpdateUser(User user)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var repo = _unitOfWork.UserRepository;
            var Uuser = await repo.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (Uuser == null)
                throw new KeyNotFoundException();
            Uuser = user;
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
        if (u == null) throw new KeyNotFoundException();

        u.Name = um.Name ?? u.Name;
        u.Email = um.Email ?? u.Email;
        _unitOfWork.UserRepository.UpdateUser(u);

        await _unitOfWork.SaveChangesAsync();
        return await GetOne(um.Id);
    }


    public async Task DeleteUserAdmin(Guid userId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var repo = _unitOfWork.UserRepository;
            var user = await repo.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new KeyNotFoundException();

            await repo.DeleteAsync(user);
            await _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransaction();
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
}