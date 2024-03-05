using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Likes;
using ArtworkSharing.DAL.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtworkSharing.Service.Services
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LikeViewModel>> Add(CreateLikeModel like)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var li = AutoMapperConfiguration.Mapper.Map<Like>(like);

                li.LikedDate = DateTime.Now;

                var likeRepository = _unitOfWork.LikeRepository;
                await likeRepository.AddAsync(li);

                await _unitOfWork.CommitTransaction();

                return await GetLikeByArtworkId(like.ArtworkId);
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
            => AutoMapperConfiguration.Mapper.Map<List<LikeViewModel>>(await _unitOfWork.LikeRepository.GetAllAsync());

        public async Task<Like> GetOne(Guid likeId)
        {
            return await _unitOfWork.LikeRepository.FindAsync(likeId);
        }

        public async Task<List<LikeViewModel>> GetLikeByArtworkId(Guid id)
            => AutoMapperConfiguration.Mapper.Map<List<LikeViewModel>>(await _unitOfWork.LikeRepository.Where(x => x.ArtworkId == id).ToListAsync());

        public async Task<bool> UnLike(Guid id)
        {
            var like = await _unitOfWork.LikeRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (like == null) return false;

            await _unitOfWork.LikeRepository.DeleteAsync(like);

            return (await _unitOfWork.SaveChangesAsync()) > 0;
        }

        public async Task Update(Like like)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var likeRepository = _unitOfWork.LikeRepository;
                var existingLike = await likeRepository.FindAsync(like.Id);
                if (existingLike == null)
                    throw new KeyNotFoundException();

                // Update like properties here if needed

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
