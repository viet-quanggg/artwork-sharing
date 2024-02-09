using ArtworkSharing.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces.Services
{
    public interface IMediaContentService
    {
        Task<IList<MediaContent>> GetAll();
        Task<MediaContent> GetOne(Guid mediaContentId);
        Task Update(MediaContent mediaContent);
        Task Add(MediaContent mediaContent);
        Task Delete(Guid mediaContentId);
    }
}
