using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.Models;
using ArtworkSharing.Core.ViewModels.Artworks;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArtworkSharing.Service.Services;

public class ArtworkService : IArtworkService
{
    private readonly IUnitOfWork _unitOfWork;

    public ArtworkService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Add(Artwork artwork)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var repos = _unitOfWork.ArtworkRepository;
            await repos.AddAsync(artwork);

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task Delete(Guid artworkId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var repos = _unitOfWork.ArtworkRepository;
            var artwork = await repos.GetAsync(a => a.Id == artworkId);
            if (artwork == null)
                throw new KeyNotFoundException();

            await repos.DeleteAsync(artwork);

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task<IList<Artwork>> GetAll()
    {
        return await _unitOfWork.ArtworkRepository.GetAllAsync();
    }

    public async Task<Artwork> GetOne(Guid artworkId)
    {

        return await _unitOfWork.ArtworkRepository.Include(x => x.Likes)
            .Include(x => x.Comments).ThenInclude(x => x.CommentedUser)!
            .Include(x => x.Artist)
            .ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == artworkId);
    }

    public async Task Update(Artwork artwork)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var repos = _unitOfWork.ArtistRepository;
            var a = await repos.FindAsync(artwork.Id);
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

    //Admin Functions
    public async Task<IList<ArtworkViewModelAdmin>> GetArtworksAdmin(int pageNumber, int pageSize)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var repos = _unitOfWork.ArtworkRepository;

            // Calculate the number of items to skip based on the page number and page size
            var itemsToSkip = (pageNumber - 1) * pageSize;

            // Query the database with pagination
            var list = await repos
                .Include(a => a.Transactions)
                .Include(a => a.Artist)
                .ThenInclude(a => a.User)
                .Include(a => a.Categories)
                .Include(a => a.MediaContents)
                .Skip(itemsToSkip)
                .Take(pageSize)
                .ToListAsync();

            // Map the result to the desired ViewModel
            return AutoMapperConfiguration.Mapper.Map<IList<ArtworkViewModelAdmin>>(list);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public async Task<ArtworkViewModelAdmin> GetArtworkAdmin(Guid artworkId)
    {
        return AutoMapperConfiguration.Mapper.Map<ArtworkViewModelAdmin>(
            await _unitOfWork.ArtworkRepository.FirstOrDefaultAsync(a => a.Id == artworkId));
    }

    public async Task<ArtworkViewModelAdmin> CreateArtworkAdmin(Guid artistId, ArtworkCreateModelAdmin acmd)
    {
        await _unitOfWork.BeginTransaction();

        var artworkProduct = AutoMapperConfiguration.Mapper.Map<Artwork>(acmd);
        artworkProduct.ArtistId = artistId;
        artworkProduct.CreatedDate = DateTime.Now;
        ;
        artworkProduct.Status = true;
        await _unitOfWork.ArtworkRepository.AddAsync(artworkProduct);

        await _unitOfWork.CommitTransaction();

        return await GetArtworkAdmin(artworkProduct.Id);
    }

    public async Task<bool> ChangeArtworkStatus_DisableAsync(Guid artworkId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var repos = _unitOfWork.ArtworkRepository;
            var updateArtwork = await repos.FirstOrDefaultAsync(ua => ua.Id == artworkId);
            if (updateArtwork == null)
            {
                throw new KeyNotFoundException();
            }

            if (updateArtwork.Status == true)
            {
                updateArtwork.Status = false;
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
                return true;
            }
            else
            {
                updateArtwork.Status = true;
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
                return true;
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ArtworkViewModelAdmin> UpdateAdmin(Guid artworkId, ArtworkUpdateModelAdmin artwork)
    {
        var updateArtwork = await _unitOfWork.ArtworkRepository.FirstOrDefaultAsync(a => a.Id == artworkId);
        if (updateArtwork == null) throw new ArgumentException(nameof(updateArtwork));

        updateArtwork.Status = artwork.Status;
        updateArtwork.Categories = artwork.Categories;
        updateArtwork.MediaContents = artwork.MediaContents;
        updateArtwork.Description = artwork.Description;
        updateArtwork.Price = artwork.Price;
        updateArtwork.Name = artwork.Name;

        _unitOfWork.ArtworkRepository.UpdateArtwork(updateArtwork);
        await _unitOfWork.SaveChangesAsync();
        return await GetArtworkAdmin(artworkId);
    }

    public async Task<bool> DeleteArtworkAdmin(Guid artworkId)
    {
        var deleteArtwork = await _unitOfWork.ArtworkRepository.FirstOrDefaultAsync(a => a.Id == artworkId);
        if (deleteArtwork == null) throw new ArgumentException(nameof(deleteArtwork));

        _unitOfWork.ArtworkRepository.DeleteAsync(deleteArtwork.Id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<List<Artwork>> GetArtworks(BrowseArtworkModel? browserArtworkModel = null!)
    {
        IQueryable<Artwork> artworks = _unitOfWork.ArtworkRepository
            .Include(x => x.Likes).AsNoTracking()
            .OrderByDescending(x => x.CreatedDate);
        if (browserArtworkModel != null)
        {
            if (browserArtworkModel.Name + "" != "") artworks = artworks.Where(x => x.Name.ToLower().Contains(browserArtworkModel.Name!.ToLower()));

            if (browserArtworkModel.Description + "" != "")
                artworks = artworks.Where(x => x.Description!.ToLower().Contains(browserArtworkModel.Description!.ToLower()));

            if (browserArtworkModel.ArtistId != null && browserArtworkModel.ArtistId != Guid.Empty) artworks = artworks.Where(x => x.ArtistId == browserArtworkModel.ArtistId);

            if (browserArtworkModel.CategoryId != null) artworks = artworks.Where(x => x.Categories != null && x.Categories!.Any(x => x.Id == browserArtworkModel.CategoryId));

            if (browserArtworkModel.IsPopular) artworks = artworks.OrderByDescending(x => x.Likes!.Count);

            if (browserArtworkModel.IsAscRecent) artworks = artworks.OrderBy(x => x.CreatedDate);

            artworks = artworks.Skip((browserArtworkModel.PageIndex) * browserArtworkModel.PageSize).Take(browserArtworkModel.PageSize);
        }
        return await artworks.Include(x => x.Comments).AsNoTracking().Include(x => x.Artist).ThenInclude(x => x.User).Include(x => x.MediaContents).AsNoTracking().ToListAsync();
    }

    public async Task<PaginatedResult> GetArtworkByArtist(Guid artistId, int pageIndex, int pageSize, string filter, string orderBy)
    {
        Expression<Func<Artwork, bool>> filterExp;
        Func<IQueryable<Artwork>, IOrderedQueryable<Artwork>> orderByExp = null;
        if (!string.IsNullOrEmpty(orderBy))
        {
            switch (orderBy)
            {
                case nameof(SortingArtwork.MostFavourite):
                    orderByExp = x => x.OrderBy(x => x.Likes != null ? x.Likes.Count : 0);
                    break;
                case nameof(SortingArtwork.RecentArtworks):
                    orderByExp = x => x.OrderByDescending(x => x.CreatedDate);
                    break;
                case nameof(SortingArtwork.PriceDescending):
                    orderByExp = x => x.OrderByDescending(x => x.Price);
                    break;
                case nameof(SortingArtwork.PriceAscending):
                    orderByExp = x => x.OrderBy(x => x.Price);
                    break;
                default:
                    break;
            }

        }

        return _unitOfWork.ArtworkRepository.GetPaginatedResult(pageSize, pageIndex, x => x.ArtistId.Equals(artistId), orderByExp, x => x.Likes, x => x.MediaContents);

    }
    public IEnumerable<Artwork> Get(Expression<Func<Artwork, bool>> filter = null,
      Func<IQueryable<Artwork>, IOrderedQueryable<Artwork>> orderBy = null, string includeProperties = "",
      int? pageIndex = null, int? pageSize = null)
    {
        try
        {
            var PackageRepository =
                _unitOfWork.ArtworkRepository.Get(filter, orderBy, includeProperties, pageIndex, pageSize);

            return PackageRepository;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<Artwork> GetMediaContentforArtwork(Guid artworkId)
    {

        return await _unitOfWork.ArtworkRepository.Include(x => x.Likes)
            .Include(x => x.Comments).ThenInclude(x => x.CommentedUser)!
            .Include(x => x.Artist)
            .ThenInclude(x => x.User)
             .Include(x => x.MediaContents)
            .FirstOrDefaultAsync(x => x.Id == artworkId);
    }

    public async Task<Artwork> GetArtwork(Guid id)
    {
        return await _unitOfWork.ArtworkRepository.FirstOrDefaultAsync(x => x.Id == id);
    }
}