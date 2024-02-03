using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class Comment : EntityBase<Guid>
    {
        public Guid CommentedUserId { get; set; }
        public Guid ArtworkId { get; set; }
        public DateTime CommentedDate { get; set; }
        public string Content { get; set; } = null!;

        public User CommentedUser { get; set; } = null!;
        public Artwork Artwork { get; set; } = null!;
    }
}
