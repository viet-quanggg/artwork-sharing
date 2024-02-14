using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Repositories
{
	public interface IFollowService
	{
		Task<IList<Follow>> GetAll();
		Task<Follow> GetOne(Guid followId);
		Task Update(Follow follow);
		Task Add(Follow follow);
		Task Delete(Guid followId);
	}
}
