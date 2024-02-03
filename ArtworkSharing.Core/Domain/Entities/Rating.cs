using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class Rating : EntityBase<Guid>
    {
        public float Star { get; set; }
        public string? Content { get; set; }
        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;
    }
}
