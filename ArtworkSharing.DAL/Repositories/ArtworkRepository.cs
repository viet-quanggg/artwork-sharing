using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.DAL.Repositories
{
    public class ArtworkRepository : Repository<Artwork>, IArtworkRepository
    {
        public ArtworkRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
