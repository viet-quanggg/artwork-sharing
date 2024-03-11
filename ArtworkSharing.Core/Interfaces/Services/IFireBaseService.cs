using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IFireBaseService
    {
        Task<string> UploadImageSingle(List<IFormFile> files);
        Task<string> UploadImage(List<IFormFile> files);
        Task<string> UploadImages(List<IFormFile> files);

        Task<string> UploadImageSingleNotList(IFormFile files);
    }
}
