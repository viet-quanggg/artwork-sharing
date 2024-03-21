using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Likes;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface ILikeService
{
    Task<List<LikeViewModel>> Add(Guid artworkId, Guid userId);


    Task Delete(Guid likeId);


    Task<List<LikeViewModel>> GetAll();


    Task<Like> GetOne(Guid likeId);


    Task<List<LikeViewModel>> GetLikeByArtworkId(Guid id);


    Task<List<LikeViewModel>> UnLike(Guid id);


    Task<List<LikeViewModel>> Update(Guid artworkId, Guid userId);

    Task<CheckLikeModel> CheckLike(Guid artworkId, Guid userId);
}