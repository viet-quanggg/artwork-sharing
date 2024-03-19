using Microsoft.AspNetCore.Http;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IFireBaseService
{
    Task<string> UploadImageSingle(List<IFormFile> files);
    Task<string> UploadImage(List<IFormFile> files);
    Task<string> UploadImages(List<IFormFile> files);

    Task<string> UploadImageSingleNotList(IFormFile files);

    Task<string> UploadImageSingleNotList(byte[] imageBytes, string imageType);
}