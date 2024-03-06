using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Repositories;

namespace ArtworkSharing.Service.Services
{
    public class FollowService : IFollowService
	{
		private readonly IUnitOfWork _unitOfWork;

		public FollowService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task Add(Follow follow)
		{
			try
			{
				await _unitOfWork.BeginTransaction();

				var repos = _unitOfWork.FollowRepository;
				await repos.AddAsync(follow);

				await _unitOfWork.CommitTransaction();
			}
			catch (Exception e)
			{
				await _unitOfWork.RollbackTransaction();
				throw;
			}
		}

		public async Task Delete(Guid followId)
		{
			try
			{
				await _unitOfWork.BeginTransaction();

				var repos = _unitOfWork.FollowRepository;
				var follow = await repos.GetAsync(f => f.Id == followId);
				if (follow == null)
					throw new KeyNotFoundException();

				await repos.DeleteAsync(follow);

				await _unitOfWork.CommitTransaction();
			}
			catch (Exception e)
			{
				await _unitOfWork.RollbackTransaction();
				throw;
			}
		}

        public async Task FollowUser(Guid currentUserId, Guid followUserId)
        {
            await _unitOfWork.FollowRepository.AddAsync(new Follow
			{
                FollowerId = currentUserId,
                FollowedId = followUserId
            });
        }

        public async Task<IList<Follow>> GetAll()
		{
			return await _unitOfWork.FollowRepository.GetAllAsync();
		}

		public async Task<Follow> GetOne(Guid followId)
		{
			return await _unitOfWork.FollowRepository.FindAsync(followId);
		}

        public async Task<bool> IsFollowing(Guid currentUserId, Guid followUserId)
        {
            var follow = await _unitOfWork.FollowRepository.GetAsync(f => f.FollowerId == currentUserId && f.FollowedId == followUserId);
			if (follow == null)
				return false;
			return true;
        }

        public async Task UnFollowUser(Guid currentUserId, Guid followUserId)
        {
			var follow = await _unitOfWork.FollowRepository.GetAsync(f => f.FollowerId == currentUserId && f.FollowedId == followUserId);
            await _unitOfWork.FollowRepository.DeleteAsync(follow);
        }

        public async Task Update(Follow follow)
		{
			try
			{
				await _unitOfWork.BeginTransaction();

				var repos = _unitOfWork.FollowRepository;
				var existingFollow = await repos.FindAsync(follow.Id);
				if (existingFollow == null)
					throw new KeyNotFoundException();

				// Update follow properties here if needed

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
