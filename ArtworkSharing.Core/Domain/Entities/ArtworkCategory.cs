using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class ArtworkCategory : EntityBase<Guid>
    {
        public Guid ArtworkId { get; set; }
        public Guid CategoryId { get; set; }
        public Artwork Artwork { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
