using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.AspNetCore.Identity;

namespace ArtworkSharing.Service.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;

    private readonly UserManager<User> _userManager;


    public AuthService(ITokenService tokenService, UserManager<User> userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<UserDto> LoginAsync(UserToLoginDto userToLoginDto)
    {
        var userMapping = AutoMapperConfiguration.Mapper.Map<User>(userToLoginDto);
        var user = await _userManager.FindByEmailAsync(userMapping.Email);

        if (user == null)
            throw new Exception("Unauthorized"); //modify here to show error to client
        var result = await _userManager.CheckPasswordAsync(user, userToLoginDto.Password);

        if (!result)
            //modify here to show error to client
            throw new Exception("Unauthorized, Invalid password");

        var userToReturn = AutoMapperConfiguration.Mapper.Map<UserDto>(userMapping);
        userToReturn.Token = await _tokenService.CreateToken(userMapping);
        return userToReturn;
    }

    public async Task<UserDto> RegisterAsync(UserToRegisterDto userToRegisterDto)
    {
        var user = AutoMapperConfiguration.Mapper.Map<User>(userToRegisterDto);
        user.UserName = userToRegisterDto.Email;
        var result = await _userManager.CreateAsync(user, userToRegisterDto.Password);
        //modify here to show error to client
        if (!result.Succeeded) throw new Exception("register fail");
        var roleResult = await _userManager.AddToRoleAsync(user, "Audience");
        //modify here to show error to client
        if (!roleResult.Succeeded) throw new Exception("add role fail"); //Rollback
        var returnUser = AutoMapperConfiguration.Mapper.Map<UserDto>(user);
        returnUser.Token = await _tokenService.CreateToken(user);
        return returnUser;
    }
}