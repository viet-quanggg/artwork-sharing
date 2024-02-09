using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IArtworkService
    {
        Task<IList<Artwork>> GetAll();
        Task<Artwork> GetOne(Guid artworkId);
        Task Update(Artwork artwork);
        Task Add(Artwork artwork);
        Task Delete(Guid artworkId);
    }
}
