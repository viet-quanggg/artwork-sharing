using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtworkSharing.Service.Services
{
	public class CommentService : ICommentService
	{
		private readonly IUnitOfWork _unitOfWork;

		public CommentService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task Add(Comment comment)
		{
			try
			{
				await _unitOfWork.BeginTransaction();

				var commentRepository = _unitOfWork.CommentRepository;
				await commentRepository.AddAsync(comment);

				await _unitOfWork.CommitTransaction();
			}
			catch (Exception e)
			{
				await _unitOfWork.RollbackTransaction();
				throw;
			}
		}

		public async Task Delete(Guid commentId)
		{
			try
			{
				await _unitOfWork.BeginTransaction();

				var commentRepository = _unitOfWork.CommentRepository;
				var comment = await commentRepository.GetAsync(c => c.Id == commentId);
				if (comment == null)
					throw new KeyNotFoundException();

				await commentRepository.DeleteAsync(comment);

				await _unitOfWork.CommitTransaction();
			}
			catch (Exception e)
			{
				await _unitOfWork.RollbackTransaction();
				throw;
			}
		}

		public async Task<IList<Comment>> GetAll()
		{
			return await _unitOfWork.CommentRepository.GetAllAsync();
		}

		public async Task<Comment> GetOne(Guid commentId)
		{
			return await _unitOfWork.CommentRepository.FindAsync(commentId);
		}

		public async Task Update(Comment comment)
		{
			try
			{
				await _unitOfWork.BeginTransaction();

				var commentRepository = _unitOfWork.CommentRepository;
				var existingComment = await commentRepository.FindAsync(comment.Id);
				if (existingComment == null)
					throw new KeyNotFoundException();

				// Update comment properties here if needed

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
