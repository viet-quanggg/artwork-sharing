using ArtworkSharing.Core.Domain.Base;
using ArtworkSharing.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class Transaction : EntityBase<Guid>
    {
        public Guid? PackageId { get; set; }
        public Guid? ArtworkId { get; set; }
        public Guid? ArtworkServiceId { get; set; }
        public Guid AudienceId { get; set; }        
        public float TotalBill { get; set; }
        public DateTime CreatedDate { get; set; }
        public Enums.TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }

        public User Audience { get; set; } = null!;
        public Artwork? Artwork { get; set; } 
        public ArtworkService? ArtworkService { get; set; }
        public Package? Package { get; set; }

    }
}
