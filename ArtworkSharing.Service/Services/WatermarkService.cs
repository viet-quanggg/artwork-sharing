using System;
using System.Net.Http;
using System.Threading.Tasks;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.Models;

using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Service.Services
{
    public class WatermarkService : IWatermarkService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IFireBaseService _fireBaseService;

        public WatermarkService(IHttpClientFactory clientFactory, IFireBaseService fireBaseService)
        {
            _clientFactory = clientFactory;
            _fireBaseService = fireBaseService;
        }

        public async Task<string> AddWatermarkAsync(string modelMainImgaUrl)
        {
            try
            {
                // Prepare the request body
                var request = new
                {
                    mainImageUrl = modelMainImgaUrl,
                    markImageUrl = "https://upload.wikimedia.org/wikipedia/commons/6/63/NU_Watermark_Logo.png",
                    markRatio = 0.5,
                    opacity = 0,
                    position = 0,
                    positionX = 0,
                    positionY = 0,
                    margin = 0
                };

                // Send POST request to QuickChart Watermark API
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("https://quickchart.io/watermark", request);

                // Check if request was successful
                response.EnsureSuccessStatusCode();
                var imageBytes = await response.Content.ReadAsByteArrayAsync();

                // Call the FireBaseService to upload the watermarked image and get the image link
                var imageUrl = await _fireBaseService.UploadImageSingleNotList(imageBytes, "jpeg");
                // Read response content as byte array
                return imageUrl;
            }
            catch (Exception)
            {
                // Log error or handle it accordingly
                throw; // You can also choose to return null or an empty byte array depending on your requirements
            }
        }
    }
}
