using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaRep.Application.Common;
using PharmaRep.Infra.Security;
using PharmaRep.Infra.Security.Models;
using System.IdentityModel.Tokens.Jwt;


namespace PharmaRep.WebAPI.Controllers
{

    [ApiVersion(1.0)]
    [Route("api/user")]
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
            var registeredUser = await _authService.RegisterAsync(user.FullName, user.Email, user.Password);

            if (registeredUser != null)
            {
                logger.LogInformation("User {email} registered successfully ", user.Email);
                return Ok(registeredUser);
            }

            logger.LogWarning("User {email} registration unsuccessful", user.Email);
            return BadRequest(new { message = "User registration unsuccessful" });
        }
    }
}
