using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Services
{
	public interface ILikeService
	{
		Task<IList<Like>> GetAll();
		Task<Like> GetOne(Guid likeId);
		Task Update(Like like);
		Task Add(Like like);
		Task Delete(Guid likeId);
	}
}
