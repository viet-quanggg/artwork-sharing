using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class LikeRepository : Repository<Like>, ILikeRepository
{
    public LikeRepository(DbContext dbContext) : base(dbContext)
    {
    }
}