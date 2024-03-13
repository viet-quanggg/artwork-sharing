using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArtworkSharing.Controllers;

[ApiController]
[Route("[controller]")]
public class PageController : ControllerBase
{
    [HttpPost(Name = "Save Page")]
    public async Task<IActionResult> SaveApiResponseToJsonFile([FromForm] int Page)
    {
        try
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("Page.json", true, true)
                .Build();
            var num = configuration.GetSection("Page").Value;
            var _configuration = (IConfigurationRoot)configuration;
            if (num.IsNullOrEmpty())
            {
                _configuration.GetSection("Page").Value = Page.ToString();
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Page.json");
                System.IO.File.WriteAllText(filePath,
                    JsonConvert.SerializeObject(_configuration.AsEnumerable(), Formatting.Indented));
            }
            return Ok("API response saved to JSON file.");
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu có
            return BadRequest($"Error: {ex.Message}");
        }
    }
}