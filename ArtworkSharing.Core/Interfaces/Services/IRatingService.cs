using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IRatingService
    {
        Task<IList<Rating>> GetAll();
        Task<Rating> GetOne(Guid ratingId);
        Task Update(Rating rating);
        Task Add(Rating rating);
        Task Delete(Guid ratingId);
    }
}
