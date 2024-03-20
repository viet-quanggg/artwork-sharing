using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Artists;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services;

public class ArtistService : IArtistService
{
    private readonly IUnitOfWork _unitOfWork;

    public ArtistService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Add(Artist artist)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var repos = _unitOfWork.ArtistRepository;
            await repos.AddAsync(artist);

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task Delete(Guid artistId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var repos = _unitOfWork.ArtistRepository;
            var artist = await repos.GetAsync(a => a.Id == artistId);
            if (artist == null)
                throw new KeyNotFoundException();

            await repos.DeleteAsync(artist);

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task<IList<Artist>> GetAll()
    {
        return await _unitOfWork.ArtistRepository.GetAllAsync();
    }

    public async Task<Artist> GetOne(Guid artistId)
    {
        return await _unitOfWork.ArtistRepository.FindAsync(artistId);
    }


    public async Task Update(Artist artist)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var repos = _unitOfWork.ArtistRepository;
            var a = await repos.FindAsync(artist.Id);
            if (a == null)
                throw new KeyNotFoundException();

            //a.Name = a.Name;

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }
    
    public async Task<ArtistProfileViewModel> GetArtistProfile(Guid artistId)
    {
        if (artistId != null)
        {
            return AutoMapperConfiguration.Mapper.Map<ArtistProfileViewModel>(await _unitOfWork.ArtistRepository
                .Include(a => a.User)
                .ThenInclude(u => u.Transactions)
                .Include(a => a.Artworks)
                .FirstOrDefaultAsync(a => a.Id == artistId));
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }
}