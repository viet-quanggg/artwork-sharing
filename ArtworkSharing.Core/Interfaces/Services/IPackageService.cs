using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
