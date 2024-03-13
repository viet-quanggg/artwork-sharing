using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class RatingRepository : Repository<Rating>, IRatingRepository
{
    public RatingRepository(DbContext dbContext) : base(dbContext)
    {
    }
}