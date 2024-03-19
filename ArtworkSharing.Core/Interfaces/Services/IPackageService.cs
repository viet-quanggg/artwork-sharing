using System.Linq.Expressions;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Package;
using ArtworkSharing.Core.ViewModels.Transactions;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IPackageService
{
    Task<IList<PackageViewModel>> GetAll();
    Task<PackageViewModel> GetOne(Guid packageId);
    Task Update(Package package);
    Task Add(Package package);
    Task Delete(Guid packageId);

    IEnumerable<Package> Get(
        Expression<Func<Package, bool>> filter = null,
        Func<IQueryable<Package>, IOrderedQueryable<Package>> orderBy = null,
        string includeProperties = "",
        int? pageIndex = null,
        int? pageSize = null
    );

    Task CheckOutPackage(TransactionViewModel transaction);
}