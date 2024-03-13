using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.MediaContent;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IMediaContentService
{
    Task<IList<MediaContentViewModel>> GetAll();
    Task<MediaContentViewModel> GetOne(Guid mediaContentId);
    Task Update(MediaContent mediaContent);
    Task Add(MediaContent mediaContent);
    Task Delete(Guid mediaContentId);
}