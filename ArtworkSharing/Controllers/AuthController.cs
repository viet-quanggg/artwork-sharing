using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;       

        public AuthController(IAuthService authService)
        {
            _authService = authService;          
        }

          
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLoginDto userToLoginDTO)
        {
            try
            {
                var user = await _authService.LoginAsync(userToLoginDTO);
                if(user == null)
                {
                    return Unauthorized();
                }
                return Ok(user);
            }          
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegisterDto userToRegisterDTO)
        {
            try
            {
                var user = await _authService.RegisterAsync(userToRegisterDTO);

                return Ok(user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }             
                
          
        }
    }
}
