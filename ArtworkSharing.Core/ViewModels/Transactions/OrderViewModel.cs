using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Domain.Enums;
using ArtworkSharing.Core.ViewModels.Artworks;
using ArtworkSharing.Core.ViewModels.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.ViewModels.Transactions
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public Guid? PackageId { get; set; }
        public Guid? ArtworkId { get; set; }
        public Guid? ArtworkServiceId { get; set; }
        public Guid AudienceId { get; set; }
        public float TotalBill { get; set; }
        public DateTime CreatedDate { get; set; }
        public TransactionType Type { get; set; }

       // public Users Audience { get; set; } = null!;
        public ArtworkViewModel? Artwork { get; set; }
        public ArtworkService? ArtworkService { get; set; }
        public PackageViewModel? Package { get; set; }
    }
}
