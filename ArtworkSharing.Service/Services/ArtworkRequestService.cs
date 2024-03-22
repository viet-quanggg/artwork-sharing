using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.DAL;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services;

public class ArtworkRequestService : IArtworkRequestService
{
    private readonly UnitOfWork _unitOfWork;

    public ArtworkRequestService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ArtworkRequestViewModel>> GetArtworkServices(int pageNumber, int pageSize)
    {
        try
        {
            var itemsToSkip = (pageNumber - 1) * pageSize;

            var list = await _unitOfWork.ArtworkServiceRepository
                .Include(a => a.Transactions)
                .Skip(itemsToSkip)
                .Take(pageSize)
                .ToListAsync();

            return AutoMapperConfiguration.Mapper.Map<List<ArtworkRequestViewModel>>(list);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public async Task<ArtworkRequestViewModel> GetArtworkService(Guid guid)
    {
        return AutoMapperConfiguration.Mapper.Map<ArtworkRequestViewModel>(
            await _unitOfWork.ArtworkServiceRepository.FirstOrDefaultAsync(a => a.Id == guid));
    }

    //User Services
    public async Task<Core.Domain.Entities.ArtworkService> CreateArtworkRequest(CreateArtworkRequestModel carm)
    {
        if (carm != null)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                var ArtworkServicerepo = _unitOfWork.ArtworkServiceRepository;

                var artworkRequest = AutoMapperConfiguration.Mapper.Map<Core.Domain.Entities.ArtworkService>(carm);

                artworkRequest.RequestedDate = DateTime.Now;
                artworkRequest.Status = ArtworkServiceStatus.Pending;
                var depositAmount = carm.RequestedPrice * (carm.RequestedDeposit / 100);

                artworkRequest.RequestedDeposit = depositAmount;

                await ArtworkServicerepo.AddAsync(artworkRequest);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
                return artworkRequest;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return null;
    }

    public async Task<List<ArtworkRequestViewModelUser>> GetArtworkRequestsByUser(Guid userId)
    {
        return AutoMapperConfiguration.Mapper.Map<List<ArtworkRequestViewModelUser>>(await _unitOfWork
            .ArtworkServiceRepository
            .Include(r => r.Artist)
            .ThenInclude(a => a.User)
            .Where(r => r.AudienceId == userId)
            .OrderByDescending(r => r.RequestedDate)
            .ToListAsync());
    }
    
    public async Task<bool> CancelArtworkRequestByUser(Guid requestId)
    {
        if (requestId != null)
        {
            await _unitOfWork.BeginTransaction();
            var repo = _unitOfWork.ArtworkServiceRepository;
            try
            {
                var artworkRequest = await repo.FirstOrDefaultAsync(ar => ar.Id == requestId);
                if (artworkRequest != null)
                {
                    artworkRequest.Status = ArtworkServiceStatus.Rejected;

                    repo.UpdateArtworkRequest(artworkRequest);
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransaction();
                    return true;
                }
                else
                {
                    // return new KeyNotFoundException();
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        return false;
    }

    public async Task<bool> ChangeStatusAfterDeposit(TransactionViewModel tvm)
    {
        if (tvm != null)
        {
            await _unitOfWork.BeginTransaction();

            var artworkRequestRepo = _unitOfWork.ArtworkServiceRepository;

            var artworkRequest = await _unitOfWork.ArtworkServiceRepository.FirstOrDefaultAsync(a => a.Id == tvm.ArtworkServiceId);
            if (artworkRequest != null)
            {
                artworkRequest.Status = ArtworkServiceStatus.InProgress;
                artworkRequestRepo.UpdateArtworkRequest(artworkRequest);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
                return true;
            }

            return false;

        }

        return false;
    }

    //User Services


    
    //Artist Services
    public async Task<List<ArtworkRequestViewModelUser>> GetArtworkRequestByArtist(Guid artistId)
    {
        return AutoMapperConfiguration.Mapper.Map<List<ArtworkRequestViewModelUser>>(await _unitOfWork
            .ArtworkServiceRepository
            .Include(r => r.Audience)
            .Where(r => r.ArtistId == artistId)
            .OrderByDescending(r => r.RequestedDate)
            .ToListAsync());
    }

    public async Task<bool> CancelArtworkRequestByArtist(Guid requestId)
    {
        if (requestId != null)
        {
            await _unitOfWork.BeginTransaction();
            var repo = _unitOfWork.ArtworkServiceRepository;
            try
            {
                var artworkRequest = await repo.FirstOrDefaultAsync(ar => ar.Id == requestId);
                if (artworkRequest != null)
                {
                    artworkRequest.Status = ArtworkServiceStatus.Rejected;

                    repo.UpdateArtworkRequest(artworkRequest);
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransaction();
                    return true;
                }
                else
                {
                    // return new KeyNotFoundException();
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        return false;
    }

    public async Task<bool> AcceptArtworkRequestByArtist(Guid requestId)
    {
        if (requestId != null)
        {
            await _unitOfWork.BeginTransaction();
            var repo = _unitOfWork.ArtworkServiceRepository;
            try
            {
                var artworkRequest = await repo.FirstOrDefaultAsync(ar => ar.Id == requestId);
                if (artworkRequest != null)
                {
                    artworkRequest.Status = ArtworkServiceStatus.Accepted;

                    repo.UpdateArtworkRequest(artworkRequest);
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransaction();
                    return true;
                }
                else
                {
                    // return new KeyNotFoundException();
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        return false;
    }
    //Artist Services

    public async Task<bool> CommitArtworkRequest(Guid id, CommitArtworkRequestModel uam)
    {
        if (id != null)
        {
            await _unitOfWork.BeginTransaction();
            var repo = _unitOfWork.ArtworkServiceRepository;
            try
            {
                var artworkRequest = await repo.FirstOrDefaultAsync(ar => ar.Id == id);
                if (artworkRequest != null)
                {
                    artworkRequest.ArtworkProduct = uam.ArtworkProduct;
                    artworkRequest.Status |= ArtworkServiceStatus.Completed;
                    repo.UpdateArtworkRequest(artworkRequest);
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransaction();
                    return true;
                }
                else
                {
                    // return new KeyNotFoundException();
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        return false;
    }

    public async Task<bool> DeleteArtworkRequest(Guid id)
    {
        var artworkService = await _unitOfWork.ArtworkServiceRepository.FirstOrDefaultAsync(_ => _.Id == id);

        if (artworkService == null) throw new ArgumentNullException(nameof(artworkService));

        await _unitOfWork.ArtworkServiceRepository.DeleteAsync(artworkService);

        var rs = await _unitOfWork.SaveChangesAsync();

        return rs > 0;
    }

   
}