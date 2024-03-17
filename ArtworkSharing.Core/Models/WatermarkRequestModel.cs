using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Models
{
    public class WatermarkRequestModel
    {
        public string MainImageUrl { get; set; }
        public string MarkImageUrl { get; set; }
        public double MarkRatio { get; set; }
        public double Opacity { get; set; }
        public string Position { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Margin { get; set; }
    }
}
