
﻿using System;
using ArtworkSharing.Core.Domain.Enums;
namespace ArtworkSharing.Core.ViewModels.Transactions
{
    public class TransactionViewModel
    {
        public Guid Id { get; set; }
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
