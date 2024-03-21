using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Models;
using ArtworkSharing.Core.ViewModels.Artworks;

namespace ArtworkSharing.Core.Interfaces.Services;

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
    Task<List<Artwork>> GetArtworks(BrowseArtworkModel? bam = null!);
    Task<PaginatedResult> GetArtworkByArtist(Guid artistId, int pageIndex, int pageSize, string filter, string orderBy);
}