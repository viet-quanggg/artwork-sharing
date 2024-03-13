using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class PackageRepository : Repository<Package>, IPackageRepository
{
    public PackageRepository(DbContext dbContext) : base(dbContext)
    {
    }
}