
using ArtworkSharing.Core.Domain.Entities;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IList<Category>> GetArtworkCategories();
        Task<Category> GetCategory(Guid CategoryId);
        Task Update(Category category);
        Task Create(Category category);
        Task Delete(Guid CategoryId);

    }
}

