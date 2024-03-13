using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageUploadController : ControllerBase
{
    private static string ApiKey = "AIzaSyB-4-AYXKKdtIMZ04WWO38cLec56loIAt0";
    private static string Bucket = "recipeorganizer-58fca.appspot.com";
    private static string AuthEmail = "recipeorganizert3@gmail.com";
    private static string AuthPassword = "recipeorganizer123";

    private readonly IHttpClientFactory _clientFactory;
    private readonly IFireBaseService _fireBaseService;

    public ImageUploadController(IHttpClientFactory clientFactory, IFireBaseService fireBaseService)
    {
        _clientFactory = clientFactory;
        _fireBaseService = fireBaseService;
    }

    [HttpPost("single")]
    public async Task<IActionResult> UploadSingleImage(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0) return BadRequest("No image uploaded");

            var link = await _fireBaseService.UploadImageSingleNotList(file);

            return Ok(link);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error uploading image: {ex.Message}");
        }
    }

    // Add methods for uploading multiple images if needed
}