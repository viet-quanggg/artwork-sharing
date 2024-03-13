using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class ArtworkRepository : Repository<Artwork>, IArtworkRepository
{
    private readonly DbContext _dbContext;

    public ArtworkRepository(DbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }


    public void UpdateArtwork(Artwork artwork)
    {
        _dbContext.Update(artwork);
    }
}