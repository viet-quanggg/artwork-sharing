using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
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

		public async Task Add(Like like)
		{
			try
			{
				await _unitOfWork.BeginTransaction();

				var likeRepository = _unitOfWork.LikeRepository;
				await likeRepository.AddAsync(like);

				await _unitOfWork.CommitTransaction();
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

		public async Task<IList<Like>> GetAll()
		{
			return await _unitOfWork.LikeRepository.GetAllAsync();
		}

		public async Task<Like> GetOne(Guid likeId)
		{
			return await _unitOfWork.LikeRepository.FindAsync(likeId);
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
