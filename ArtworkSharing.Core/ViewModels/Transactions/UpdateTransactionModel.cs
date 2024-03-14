using System;

namespace ArtworkSharing.Core.ViewModels.Transactions
{
    public class UpdateTransactionModel
    {
        public Guid? PackageId { get; set; }
        public Guid? ArtworkId { get; set; }
        public Guid? ArtworkServiceId { get; set; }
        public Guid AudienceId { get; set; }
        public float TotalBill { get; set; }
        public DateTime CreatedDate { get; set; }
        public Domain.Enums.TransactionStatus Status { get; set; }

        public Domain.Enums.TransactionType Type { get; set; }

        // Add whatever you need
    }
}
