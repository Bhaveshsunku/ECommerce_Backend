using ECommerce.Business.DTOs.Auth;
using ECommerce.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterDto dto)
    {
        var result =
            await _authService.RegisterAsync(dto);

        return Ok(result);
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginDto dto)
    {
        var result =
            await _authService.LoginAsync(dto);

        if (result == null)
        {
            return Unauthorized(
                "Invalid email or password.");
        }

        return Ok(result);
    }
}