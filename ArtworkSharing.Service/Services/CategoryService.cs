using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL.Extensions;

namespace ArtworkSharing.Service.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Create(Category category)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var repo = _unitOfWork.CategoryRepository;
            await repo.AddAsync(category);

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            _unitOfWork.RollbackTransaction();
            throw new Exception();
        }
    }

    public async Task Delete(Guid CategoryId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var repo = _unitOfWork.CategoryRepository;
            var category = await repo.FirstOrDefaultAsync(c => c.Id == CategoryId);
            if (category == null) throw new KeyNotFoundException();

            await repo.DeleteAsync(category);
            await _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransaction();
            throw new Exception();
        }
    }

    public async Task<IList<Category>> GetArtworkCategories()
    {
        return await _unitOfWork.CategoryRepository.GetAllAsync();
    }

    public async Task<Category> GetCategory(Guid CategoryId)
    {
        return await _unitOfWork.CategoryRepository.FirstAsync(c => c.Id == CategoryId);
    }

    public async Task Update(Category category)
    {
        try
        {
            await _unitOfWork.BeginTransaction();
            var repo = _unitOfWork.CategoryRepository;
            var Ucategory = await repo.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (Ucategory == null)
                throw new KeyNotFoundException();
            Ucategory = category;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }
}