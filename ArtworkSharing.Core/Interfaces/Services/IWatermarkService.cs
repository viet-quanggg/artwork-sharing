using ArtworkSharing.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IWatermarkService
    {
        Task<byte[]> AddWatermarkAsync(WatermarkRequestModel model);
    }
}