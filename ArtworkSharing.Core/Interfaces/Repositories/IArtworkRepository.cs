using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Repositories;

public interface IArtworkRepository : IRepository<Artwork>
{
    void UpdateArtwork(Artwork artwork);
}