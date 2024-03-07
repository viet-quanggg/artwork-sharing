using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ArtworkSharing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PageController : Microsoft.AspNetCore.Mvc.Controller
    {
        public PageController() { 
            
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost(Name = "Save Page")]
        public async Task<IActionResult> SaveApiResponseToJsonFile([FromForm] int Page)
        {

            try
            {

                IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("Page.json", true, true)
                .Build();
                var num = configuration.GetSection("Page").Value;
                IConfigurationRoot _configuration = (IConfigurationRoot)configuration;
                if (num.IsNullOrEmpty())
                {
                    _configuration.GetSection("Page").Value = Page.ToString();
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Page.json");
                    System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(_configuration.AsEnumerable(), Newtonsoft.Json.Formatting.Indented));

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
}
