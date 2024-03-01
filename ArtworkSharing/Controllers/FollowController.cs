using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        [HttpPost("follow")]
        public async Task<IActionResult> FollowCreator() 
        {

            return Ok();
        }
    }
}
