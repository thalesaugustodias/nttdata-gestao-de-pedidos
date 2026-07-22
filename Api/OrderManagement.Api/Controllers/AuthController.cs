using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrderManagement.Api.Requests;
using OrderManagement.Api.Services;
using OrderManagement.Api.Settings;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IOptions<AuthSettings> authSettings, ITokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var auth = authSettings.Value;

        if (request.Email != auth.Email || request.Password != auth.Password)
            return Unauthorized(new { message = "Invalid credentials." });

        var token = tokenService.GenerateToken(auth.Email, auth.CustomerId);

        return Ok(new { token });
    }
}

