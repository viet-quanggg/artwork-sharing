using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.ViewModels.Package
{
    public class PackageViewModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public float Price { get; set; }
        public int Duration { get; set; }
    }
}
