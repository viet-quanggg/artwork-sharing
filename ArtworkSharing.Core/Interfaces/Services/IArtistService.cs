using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IArtistService
{
    Task<IList<Artist>> GetAll();
    Task<Artist> GetOne(Guid artistId);
    Task Update(Artist artist);
    Task Add(Artist artist);
    Task Delete(Guid artist);
    Task<Artist> GetnameArtist(Guid artistId);
    Task<IList<Artist>> GetAllField();
}