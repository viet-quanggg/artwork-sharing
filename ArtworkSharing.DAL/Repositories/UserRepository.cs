using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly DbContext _dbContext;

    public UserRepository(DbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void UpdateUser(User user)
    {
        _dbContext.Update(user);
    }
}