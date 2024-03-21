using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.User;
using ArtworkSharing.Core.ViewModels.Users;
using ArtworkSharing.Service.AutoMappings;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ArtworkSharing.Controllers;

[ApiController]
[Route("api/usercontroller")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService service)
    {
        _userService = service;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllUsers(int pageNumber, int pageSize)
    {
        try
        {
            var userList = await _userService.GetUsers(pageNumber, pageSize);
            return Ok(userList);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpGet("getuser")]
    public async Task<ActionResult> getUser(Guid userId)
    {
        try
        {
            var user = await _userService.GetUserAdmin(userId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost("createuser")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserViewModel cum)
    {
        try
        {
            var user = AutoMapperConfiguration.Mapper.Map<Core.Domain.Entities.User>(cum);
            _userService.CreateNewUser(user);
            return Ok(user);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpDelete("deleteuser")]
    public async Task<ActionResult> DeleteUser(Guid userId)
    {
        try
        {
            var user = await _userService.GetUserAdmin(userId);
            if (user == null)
                return NotFound("User not found");
            await _userService.DeleteUser(userId);

            return Ok("Deleted!");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult> UpdateUser([FromRoute] Guid userId, UpdateUserModelAdmin uuma)
    {
        if (userId == Guid.Empty || uuma == null) return BadRequest(new { Message = "User not found!" });
        return Ok(await _userService.UpdateUser(userId, uuma));
    }



    /// <summary>
    /// update user (name and phone)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateUserModel"></param>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<ActionResult> UpdateUser(UpdateUserModel updateUserModel)
    {
        var id = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId);

        if (id == null) return BadRequest();

        if (Guid.Parse(id + "") == Guid.Empty || updateUserModel == null) return BadRequest(new { Message = "User not found!" });

        return Ok(await _userService.UpdateUser(Guid.Parse(id + ""), updateUserModel));
    }


    /// <summary>
    /// Get user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetUserAuth()
    {
        var u = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId);

        if (u == null) return BadRequest();
        return Ok(await _userService.GetUserAdmin(Guid.Parse(u + "")));
    }

}