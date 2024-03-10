using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Artworks;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services
{
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
            return await _unitOfWork.ArtworkRepository.FindAsync(artworkId);
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
        public async Task<List<Artwork>> GetArtworks(BrowseArtworkModel? bam = null!)
        {
            IQueryable<Artwork> artworks = _unitOfWork.ArtworkRepository.GetAll().OrderByDescending(x => x.CreatedDate).AsQueryable();

            if (bam != null)
            {
                if (bam.Name + "" != "")
                {
                    artworks = artworks.Where(x => x.Name.ToLower().Contains(bam.Name!.ToLower()));
                }
                if (bam.Description + "" != "")
                {
                    artworks = artworks.Where(x => x.Description!.ToLower().Contains(bam.Description!.ToLower()));
                }
                if (bam.IsPopular)
                {
                    artworks = artworks.OrderByDescending(x => x.Likes!.Count);
                }
                if (bam.IsAscRecent)
                {
                    artworks = artworks.OrderBy(x => x.CreatedDate);
                }
                if (bam.ArtistId != Guid.Empty)
                {
                    artworks = artworks.Where(x => x.ArtistId == bam.ArtistId);
                }
            }
            return await artworks.ToListAsync();
        }
    }
}
