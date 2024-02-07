using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class Like : EntityBase<Guid>
    {
        public Guid LikedUserId { get; set; }
        public Guid ArtworkId { get; set; }
        public DateTime LikedDate { get; set; }
        public User? LikedUser { get; set; }
        public Artwork? Artwork { get; set; }
    }
}
