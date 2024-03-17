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

        public WatermarkService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<byte[]> AddWatermarkAsync(WatermarkRequestModel model)
        {
            try
            {
                // Prepare the request body
                var request = new
                {
                    mainImageUrl = model.MainImageUrl,
                    markImageUrl = "",
                    markRatio = model.MarkRatio,
                    opacity = model.Opacity,
                    position = model.Position,
                    positionX = model.PositionX,
                    positionY = model.PositionY,
                    margin = model.Margin
                };

                // Send POST request to QuickChart Watermark API
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("https://quickchart.io/watermark", request);

                // Check if request was successful
                response.EnsureSuccessStatusCode();

                // Read response content as byte array
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception)
            {
                // Log error or handle it accordingly
                throw; // You can also choose to return null or an empty byte array depending on your requirements
            }
        }
    }
}
