using APIs.Models;
using APIs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(UserDto request)
    {
        if (request.Username == "" || request.Password == "")
            return BadRequest("Username and Password are required");

        var user = await authService.RegisterAsync(request);

        return user is null ? BadRequest("User already exists") : Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
    {
        if (request.Username == "" || request.Password == "")
            return BadRequest("Username and Password are required");

        var response = await authService.LoginAsync(request);

        return response is null ? BadRequest("Invalid username or password") : Ok(response);
    }

    [Authorize]
    [HttpGet]
    public ActionResult<string> AuthenticatedOnlyEndpoint()
    {
        return "Authentication successful.";
    }

    [Authorize]
    [HttpGet("admin-only")]
    public ActionResult<string> AdminOnlyEndpoint()
    {
        return "Admin authorization granted.";
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
    {
        var token = await authService.RefreshTokenAsync(request);
        if (token is null)
            return Unauthorized("Expired or unauthorized token.");
        return Ok(token);
    }
}