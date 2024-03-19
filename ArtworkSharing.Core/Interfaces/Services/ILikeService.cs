using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Likes;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface ILikeService
{
    Task<List<LikeViewModel>> Add(LikeModel like);


    Task Delete(Guid likeId);


    Task<List<LikeViewModel>> GetAll();


    Task<Like> GetOne(Guid likeId);


    Task<List<LikeViewModel>> GetLikeByArtworkId(Guid id);


    Task<List<LikeViewModel>> UnLike(Guid id);


    Task<List<LikeViewModel>> Update(LikeModel like);

    Task<CheckLikeModel> CheckLike(LikeModel lm);
}