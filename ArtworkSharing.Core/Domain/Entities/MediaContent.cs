using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class MediaContent : EntityBase<Guid>
    {
        public Guid ArtworkId { get; set; }
        public float Capacity { get; set; }
        public string Media { get; set; } = null!;

        public Artwork Artwork { get; set; } = null!;
    }
}
