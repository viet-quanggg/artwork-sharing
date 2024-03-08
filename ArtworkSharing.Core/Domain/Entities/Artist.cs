using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class Artist : EntityBase<Guid>
    {
        public Guid UserId { get; set; }
        public string BankAccount { get; set; }
        public ICollection<ArtworkService>? ArtworkServices { get; set; }
        public ICollection<Artwork>? Artworks { get; set; }
        public ICollection<ArtistPackage>? ArtistPackages { get; set; }

        public User User { get; set; } = null!;
    }
}
