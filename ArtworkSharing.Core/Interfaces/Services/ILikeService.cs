using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Likes;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface ILikeService
    {
        Task<List<LikeViewModel>> Add(CreateLikeModel like);


        Task Delete(Guid likeId);


        Task<List<LikeViewModel>> GetAll();


        Task<Like> GetOne(Guid likeId);


        Task<List<LikeViewModel>> GetLikeByArtworkId(Guid id);


        Task<bool> UnLike(Guid id);


        Task Update(Like like);
    }
}
