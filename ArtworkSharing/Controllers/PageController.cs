using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArtworkSharing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PageController : ControllerBase
    {
        public PageController() { 
            
        }

        [HttpPost(Name = "Save Page")]
        public async Task<IActionResult> SaveApiResponseToJsonFile([FromForm] int Page)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Page.json");

                if (!filePath.IsNullOrEmpty())
                {
                    var jsonString = System.IO.File.ReadAllText(filePath);

                    JObject jsonObject;
                    if (jsonString.StartsWith("["))
                    {
                        // JSON file contains an array
                        jsonObject = JObject.Parse(jsonString);
                        JArray jsonArray = jsonObject["PageArray"] as JArray;

                        foreach (JObject obj in jsonArray)
                        {
                            if (obj["Key"].ToString() == "Page")
                            {
                                obj["Value"] = Page.ToString();
                                break;
                            }
                        }
                    }
                    else
                    {
                        // JSON file contains an object
                        jsonObject = JObject.Parse(jsonString);

                        if (jsonObject.ContainsKey("Page"))
                        {
                            jsonObject["Page"]["Value"] = Page.ToString();
                        }
                        else
                        {
                            jsonObject.Add(new JProperty("Page", new JObject(new JProperty("Key", "Page"), new JProperty("Value", Page.ToString()))));
                        }
                    }

                    System.IO.File.WriteAllText(filePath, jsonObject.ToString());
                }
                else
                {
                    JObject jsonObject = new JObject
            {
                { "PageArray", new JArray(new JObject(new JProperty("Key", "Page"), new JProperty("Value", Page.ToString())))}
            };

                    System.IO.File.WriteAllText(filePath, jsonObject.ToString());
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
