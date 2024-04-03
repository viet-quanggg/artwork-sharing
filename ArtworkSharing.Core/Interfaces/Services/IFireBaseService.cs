using Microsoft.AspNetCore.Http;

namespace ArtworkSharing.Core.Interfaces.Services;

public interface IFireBaseService
{
    Task<string> Test(List<IFormFile> files);
    Task<string> Test2(List<IFormFile> files);
    Task<List<string>> UploadMultiImagesAsync(List<IFormFile> files);

    Task<string> UploadImageSingle(IFormFile files);

    Task<string> UploadImageWatermarkIntoFireBase(byte[] imageBytes, string imageType);
}