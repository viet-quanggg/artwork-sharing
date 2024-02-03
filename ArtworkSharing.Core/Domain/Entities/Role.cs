using ArtworkSharing.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class Role : EntityBase<Guid>
    {
        public string Name { get; set; } = null!;
       
    }
}
