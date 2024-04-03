using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class ArtistPackageRepository : Repository<ArtistPackage>, IArtistPackageRepository
{
    public ArtistPackageRepository(DbContext dbContext) : base(dbContext)
    {
    }
}