using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Likes;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.Service.Services;

public class LikeService : ILikeService
{
    private readonly IUnitOfWork _unitOfWork;

    public LikeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<LikeViewModel>> Add(Guid artworkId, Guid userId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            Like li = new Like
            {
                ArtworkId = artworkId,
                UserId = userId
            };

            li.LikedDate = DateTime.Now;

            var likeRepository = _unitOfWork.LikeRepository;
            await likeRepository.AddAsync(li);

            await _unitOfWork.CommitTransaction();

            return await GetLikeByArtworkId(li.ArtworkId);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task Delete(Guid likeId)
    {
        try
        {
            await _unitOfWork.BeginTransaction();

            var likeRepository = _unitOfWork.LikeRepository;
            var like = await likeRepository.GetAsync(l => l.Id == likeId);
            if (like == null)
                throw new KeyNotFoundException();

            await likeRepository.DeleteAsync(like);

            await _unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task<List<LikeViewModel>> GetAll()
    {
        return AutoMapperConfiguration.Mapper.Map<List<LikeViewModel>>(await _unitOfWork.LikeRepository.GetAllAsync());
    }

    public async Task<Like> GetOne(Guid likeId)
    {
        return await _unitOfWork.LikeRepository.FindAsync(likeId);
    }

    public async Task<List<LikeViewModel>> GetLikeByArtworkId(Guid id)
    {
        return AutoMapperConfiguration.Mapper.Map<List<LikeViewModel>>(await _unitOfWork.LikeRepository
            .Where(x => x.ArtworkId == id).ToListAsync());
    }

    public async Task<List<LikeViewModel>> UnLike(Guid id)
    {
        var like = await _unitOfWork.LikeRepository.FirstOrDefaultAsync(x => x.Id == id);
        if (like == null) return null!;

        var artworkId = like.ArtworkId;

        await _unitOfWork.LikeRepository.DeleteAsync(like);

        return await GetLikeByArtworkId(artworkId);
    }
    public async Task<CheckLikeModel> CheckLike(Guid artworkId, Guid userId)
    {
        var like = await _unitOfWork.LikeRepository.FirstOrDefaultAsync(x =>
           x.UserId == userId && x.ArtworkId == artworkId);
        return new CheckLikeModel { LikeViewModels = await GetLikeByArtworkId(artworkId), Result = like != null };
    }

    public async Task<List<LikeViewModel>> Update(Guid artworkId, Guid userId)
    {
        var like = await _unitOfWork.LikeRepository.FirstOrDefaultAsync(x =>
            x.UserId == userId && x.ArtworkId == artworkId);

        if (like == null)
            return await Add(artworkId, userId);
        return await UnLike(like.Id);
    }
}