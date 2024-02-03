using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class ArtistPackage : EntityBase<Guid>
    {
        public Guid ArtistId { get; set; }
        public Guid PackageId { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime PurchasedDate { get; set; }              

        public Artist Artist { get; set; } = null!;
        public Package Package { get; set; } = null!;
        public Transaction Transaction { get; set; } = null!;
    }
}
