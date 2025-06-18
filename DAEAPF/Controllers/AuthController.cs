using Microsoft.AspNetCore.Mvc;
using DAEAPF.Application.DTOs;
using DAEAPF.Infrastructure.Encryptor;
using DAEAPF.Infrastructure.JWT;
using Microsoft.EntityFrameworkCore;
using DAEAPF.Infrastructure.Context;
using DAEAPF.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace DAEAPF.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly NegociosAppContext _context;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtService _jwtService;

    public AuthController(NegociosAppContext context, IPasswordHasher hasher, IJwtService jwtService)
    {
        _context = context;
        _hasher = hasher;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        var exists = await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo);
        if (exists)
            return BadRequest("Este correo ya está registrado.");

        var user = new Usuario
        {
            Nombre = dto.Nombre,
            Correo = dto.Correo,
            ContrasenaHash = _hasher.Hash(dto.Contrasena),
            Rol = dto.Rol
        };

        _context.Usuarios.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Usuario registrado exitosamente." });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == dto.Correo);

        if (user == null || !_hasher.Verify(dto.Contrasena, user.ContrasenaHash))
            return Unauthorized("Correo o contraseña incorrectos.");

        var token = _jwtService.GenerateToken(user.Id.ToString(), user.Correo, user.Rol);

        return Ok(new
        {
            token,
            user = new { user.Id, user.Nombre, user.Correo, user.Rol }
        });
    }
}