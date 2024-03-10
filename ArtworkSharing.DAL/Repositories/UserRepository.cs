using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public void UpdateUser(User u)
        {
            _context.Entry(u).State = EntityState.Modified;
        }
    }
}

