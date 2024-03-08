using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IPackageService
    {
        Task<IList<Package>> GetAll();
        Task<Package> GetOne(Guid packageId);
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
    }
}
