using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        void UpdateUser(User u);
    }
}
