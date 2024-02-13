using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Services
{
	public interface ICommentService
	{
		Task<IList<Comment>> GetAll();
		Task<Comment> GetOne(Guid commentId);
		Task Update(Comment comment);
		Task Add(Comment comment);
		Task Delete(Guid commentId);
	}
}
