using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class Follow : EntityBase<Guid>
    {
        public Guid FollowerId { get; set; }
        public Guid FollowedId { get; set; }

        public User Follower { get; set; } = null!;
        public User Followed { get; set; } = null!;
    }
}
