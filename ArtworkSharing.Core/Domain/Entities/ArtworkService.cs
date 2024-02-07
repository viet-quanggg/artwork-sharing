using ArtworkSharing.Core.Domain.Base;
using ArtworkSharing.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class ArtworkService : EntityBase<Guid>
    {
        public Guid AudienceId { get; set; }
        public Guid ArtistId { get; set; }
        public Guid TransactionId { get; set; }
        public string? Description { get; set; }
        public DateTime RequestedDate { get; set; }
        public float RequestedPrice { get; set; }
        public float RequestedDeposit { get; set; }
        public DateTime RequestedDeadlineDate { get; set; }
        public ArtworkServiceStatus Status { get; set; }
        public User Audience { get; set; } = null!;
        public Artist Artist { get; set; } = null!;
        public ICollection<MediaContent>? ArtworkProduct { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
