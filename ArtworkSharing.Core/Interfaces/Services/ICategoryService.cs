﻿using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IList<Category>> GetCategories();
        Task<Category> GetCategory(Guid categoryId);
        Task UpdateCategory(Category category);
        Task CreateCategory(Category category);
        Task DeleteCategory(Guid categoryId);
    }
}