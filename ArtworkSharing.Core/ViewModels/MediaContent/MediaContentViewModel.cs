using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.ViewModels.MediaContent
{
    public class MediaContentViewModel
    {
        public Guid ArtworkId { get; set; }
        public float Capacity { get; set; }
        public string Media { get; set; } = null!;

        public Artwork Artwork { get; set; } = null!;
    }
}
