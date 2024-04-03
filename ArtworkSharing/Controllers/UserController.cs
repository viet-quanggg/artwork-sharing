using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.User;
using ArtworkSharing.Core.ViewModels.Users;
using ArtworkSharing.Extensions;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult> GetAllUsers()
    {
        try
        {
            var userList = await _userService.GetUsers();
            return Ok(userList);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("getuser")]
    public async Task<ActionResult> getUser()
    {
        try
        {
            var id = HttpContext.Items["UserId"];
            if (id == null) return Unauthorized();

            Guid uid = Guid.Parse(id + "");

            if (uid == Guid.Empty) return Unauthorized();

            var user = await _userService.GetUserAdmin(uid);
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
            var user = AutoMapperConfiguration.Mapper.Map<User>(cum);
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

    [HttpPut("/ChangeUserStatus/{userId}")]
    public async Task<IActionResult> ChangeUserStatus(Guid userId)
    {
        if (userId == Guid.Empty) return BadRequest(new { Message = "User not found!" });
        return Ok(await _userService.ChangeUserStatus(userId));
    }



    /// <summary>
    /// update user (name and phone)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateUserModel"></param>
    /// <returns></returns>
    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateUser([FromRoute] Guid id, UpdateUserModel updateUserModel)
    {
        if (id == Guid.Empty || updateUserModel == null) return BadRequest(new { Message = "User not found!" });
        return Ok(await _userService.UpdateUser(id, updateUserModel));
    }

}