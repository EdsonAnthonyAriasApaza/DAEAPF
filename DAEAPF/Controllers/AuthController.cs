using Microsoft.AspNetCore.Mvc;
using DAEAPF.Application.DTOs;
using DAEAPF.Application.DTOs.Usuarios;
using DAEAPF.Application.Interfaces.Services;
using DAEAPF.Application.Interfaces.Services.Usuarios;
using Microsoft.AspNetCore.Authorization;

namespace DAEAPF.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _authService.RegisterAsync(dto);
        return Ok(new { message = "Usuario registrado exitosamente." });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }
}