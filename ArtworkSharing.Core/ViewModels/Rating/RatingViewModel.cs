using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.ViewModels.Rating
{
    public class RatingViewModel
    {
        public float Star { get; set; }
        public string? Content { get; set; }
        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;
    }
}
