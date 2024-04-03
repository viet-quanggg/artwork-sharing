using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Rating;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IRatingService
{
    Task<IList<RatingViewModel>> GetAll();
    Task<RatingViewModel> GetOne(Guid ratingId);
    Task Update(Rating rating);
    Task Add(Rating rating);
    Task Delete(Guid ratingId);
}