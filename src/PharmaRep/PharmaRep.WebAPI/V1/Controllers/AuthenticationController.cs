using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaRep.Infra.Security;
using PharmaRep.Infra.Security.Models;


namespace PharmaRep.WebAPI.Controllers;


[ApiVersion(1.0)]
[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    // POST: auth/login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser user)
    {
        var loggedInUser = await _authService.LoginAsync(user.Email, user.Password);

        if (loggedInUser != null)
        {
            logger.LogInformation("User {email} logged in successfully", user.Email);
            return Ok(loggedInUser);
        }

        logger.LogWarning("User {email} login unsuccessful", user.Email);
        return BadRequest(new { message = "User login unsuccessful" });
    }

    // POST: auth/register
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] RegisterUser user)
    {
        var response = await _authService.RegisterAsync(user.FullName, user.Email, user.Password);

        if (response!.IsSuccess)
        {
            var registeredUser = response.RegisteredUser;
            logger.LogInformation("User {email} registered successfully ", user.Email);
            return Ok(registeredUser);
        }
        else
        {
            logger.LogWarning(response.ErrorMessage);
            return BadRequest(new { message = response.ErrorMessage });
        }
    }
}
