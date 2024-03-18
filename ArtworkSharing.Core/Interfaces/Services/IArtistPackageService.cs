using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IArtistPackageService
{
    Task<IList<ArtistPackage>> GetAll();
    Task<ArtistPackage> GetOne(Guid artistPackageId);
    Task Update(ArtistPackage artistPackage);
    Task Add(ArtistPackage artistPackage);
    Task Delete(Guid artistPackageId);

    Task CheckAndUpdatePackageExpiration();
}