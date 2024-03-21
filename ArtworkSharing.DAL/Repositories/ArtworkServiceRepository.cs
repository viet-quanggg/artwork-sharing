using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class ArtworkServiceRepository : Repository<ArtworkService>, IArtworkServiceRepository
{
    private readonly DbContext _dbContext;
    public ArtworkServiceRepository(DbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void UpdateArtworkRequest(ArtworkService artworkService)
    {
        var entry = _dbContext.Entry(artworkService);
        if (entry.State == EntityState.Detached)
        {
            _dbContext.Attach(artworkService);
        }
        _dbContext.Entry(artworkService).State = EntityState.Modified;    }
    

}