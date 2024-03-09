using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.Core.ViewModels.Artworks;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IArtworkService
    {
        Task<IList<Artwork>> GetAll();
        Task<Artwork> GetOne(Guid artworkId);
        Task Update(Artwork artwork);
        Task Add(Artwork artwork);
        Task Delete(Guid artworkId);

        //Admin Functions
        Task<IList<ArtworkViewModelAdmin>> GetArtworksAdmin(int pageNumber, int pageSize);
        Task<ArtworkViewModelAdmin> GetArtworkAdmin(Guid artworkId);
        Task<ArtworkViewModelAdmin> CreateArtworkAdmin(Guid artistId, ArtworkCreateModelAdmin acmd);
        Task<bool> ChangeArtworkStatus_DisableAsync(Guid guid);
        Task<ArtworkViewModelAdmin> UpdateAdmin(Guid artworkId, ArtworkUpdateModelAdmin artwork);
        Task<bool> DeleteArtworkAdmin(Guid artworkId);
    }
}
