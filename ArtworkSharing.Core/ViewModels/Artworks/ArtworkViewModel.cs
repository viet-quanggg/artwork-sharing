using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.ViewModels.Artworks
{
    public class ArtworkViewModel
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
        public ICollection<Category>? Categories { get; set; }
    }
}
