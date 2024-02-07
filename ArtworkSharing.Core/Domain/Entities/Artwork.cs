using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class Artwork : EntityBase<Guid>
    {
        public Guid ArtistId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public float Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }

        public Artist Artist { get; set; } = null!;
        public ICollection<MediaContent> MediaContents { get; set; } = null!;
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public ICollection<ArtworkCategory>? Categories { get; set; }
    }
}
