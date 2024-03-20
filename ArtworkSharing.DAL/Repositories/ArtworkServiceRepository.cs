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
    
}