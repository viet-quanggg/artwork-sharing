using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class ArtworkServiceRepository : Repository<ArtworkService>, IArtworkServiceRepository
{
    public ArtworkServiceRepository(DbContext dbContext) : base(dbContext)
    {
    }
}