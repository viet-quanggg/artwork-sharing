using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class PaypalItem:EntityBase<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string CurrencyCode { get; set; }
        public double Value { get; set; }
        public Guid PaypalOrderId { get; set; }

        public PaypalOrder PaypalOrder { get; set; }
    }
}
