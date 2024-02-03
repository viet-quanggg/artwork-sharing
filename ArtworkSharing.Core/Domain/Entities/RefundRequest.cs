using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class RefundRequest : EntityBase<Guid>
    {
        public Guid TransactionId { get; set; }
        public DateTime RefundRequestDate { get; set; }
        public string Description { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public Transaction Transaction { get; set; } = null!;
    }
}
