using System.Security.Claims;
using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.Models;
using ArtworkSharing.Service.AutoMappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtworkSharing.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IEmailSender _emailSender;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public AuthController(IEmailSender emailSender,
        UserManager<User> userManager,
        ITokenService tokenService,
        SignInManager<User> signInManager)
    {
        _emailSender = emailSender;
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserToLoginDto userToLoginDTO)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userToLoginDTO.Email);

            if (user == null)
                return Unauthorized($"Invalid user with email {userToLoginDTO.Email}");

            var isEmailActivated = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailActivated)
                return Unauthorized("Please confirm your email first");

            var result = await _signInManager.CheckPasswordSignInAsync(user, userToLoginDTO.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid password");
            var userMapping = AutoMapperConfiguration.Mapper.Map<User>(userToLoginDTO);
            var userToReturn = AutoMapperConfiguration.Mapper.Map<UserDto>(userMapping);
            userToReturn.Token = await _tokenService.CreateToken(userMapping);

            return Ok(userToReturn);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(UserToRegisterDto userToRegisterDTO)
    {
        try
        {
            var user = AutoMapperConfiguration.Mapper.Map<User>(userToRegisterDTO);
            user.UserName = userToRegisterDTO.Email;
            var result = await _userManager.CreateAsync(user, userToRegisterDTO.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);
            var roleResult = await _userManager.AddToRoleAsync(user, "Audience");

            if (!roleResult.Succeeded) return BadRequest(result.Errors); //Rollback added user

            var returnUser = AutoMapperConfiguration.Mapper.Map<UserDto>(user);
            returnUser.Token = await _tokenService.CreateToken(user);

            //send confirmation email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>", true);


            return Ok(returnUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null) return BadRequest("Invalid user or code");
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest("Invalid user");
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded) return Ok("Email confirmed");
        return BadRequest("Invalid code");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResendConfirmationEmail(string Email)
    {
        var user = await _userManager.FindByEmailAsync(Email);
        if (user == null) return BadRequest("Invalid email");
        if (await _userManager.IsEmailConfirmedAsync(user)) return BadRequest("Email already confirmed");
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token }, Request.Scheme);
        await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
            $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>", true);
        return Ok("Email sent");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                // Don't reveal that the user does not exist or is not confirmed
                return BadRequest();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                GetEmailBodyForResetPassword(callbackUrl), true);

            return Ok("Email sent");
        }

        return BadRequest("Invalid email");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string token = null)
    {
        if (token == null) return BadRequest("A code must be supplied for password reset");
        return Ok(new ResetPasswordModel { Code = token });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid model");
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return BadRequest("User not found. Please check your email address.");
        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        if (result.Succeeded) return Ok("Password reset");
        return BadRequest(result.Errors);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
    {
        if (remoteError != null) return BadRequest($"Error from external provider: {remoteError}");
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null) return BadRequest("Error loading external login information.");
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
        if (result.Succeeded) return Ok("Login successful");
        if (result.IsLockedOut)
        {
            return BadRequest("User is locked out");
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var user = new User { UserName = email, Email = email };
        var createResult = await _userManager.CreateAsync(user);
        if (createResult.Succeeded)
        {
            createResult = await _userManager.AddLoginAsync(user, info);
            if (createResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok("Login successful");
            }
        }

        AddErrors(createResult);
        return BadRequest("Error creating user");
    }


    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
    }

    private string GetEmailBodyForResetPassword(string callbackUrl)
    {
        return $@"<html>
        <body>
            <p>Hello,</p>
            <p>You recently requested to reset your password. Please click the following link to reset your password:</p>
            <p><a href='{callbackUrl}'>Reset your password</a></p>
            <p>If you didn't request this, you can safely ignore this email.</p>
            <p>Regards,<br/>ArtworkSharing</p>
        </body>
    </html>";
    }
}