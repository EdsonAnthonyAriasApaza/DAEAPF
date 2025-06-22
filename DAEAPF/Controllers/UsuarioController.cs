using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAEAPF.Domain.Models; // ajusta si tu modelo está en otro namespace
using DAEAPF.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization; // ajusta si tu DbContext está aquí
using DAEAPF.Infrastructure.Encryptor;

namespace DAEAPF.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly NegociosAppContext _context;
    private readonly IPasswordHasher _hasher;

    public UsuarioController(NegociosAppContext context, IPasswordHasher hasher)
    {
        _context = context;
        _hasher = hasher;
    }

    // GET: api/Usuario
    
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return Ok(usuarios);
    }

    // GET: api/Usuario/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound();
        return Ok(usuario);
    }

    // POST: api/Usuario
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    // PUT: api/Usuario/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] Usuario usuario)
    {
        var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (userIdFromToken == null)
            return Unauthorized();

        // Si no es admin, solo puede modificar su propio perfil
        if (userRole != "admin" && userIdFromToken != id.ToString())
            return Forbid(); // 403 - No tiene permiso

        // Verifica que el ID de la URL y el del body coincidan
        if (id != usuario.Id)
            return BadRequest();

        var usuarioExistente = await _context.Usuarios.FindAsync(id);
        if (usuarioExistente == null)
            return NotFound();

        // Actualizar campos
        usuarioExistente.Nombre = usuario.Nombre;
        usuarioExistente.Correo = usuario.Correo;

        // Cifrar contraseña antes de guardar
        usuarioExistente.ContrasenaHash = _hasher.Hash(usuario.ContrasenaHash);

        // Solo un admin puede cambiar el rol
        if (userRole == "admin")
            usuarioExistente.Rol = usuario.Rol;

        await _context.SaveChangesAsync();
        return NoContent();
    }



    // DELETE: api/Usuario/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound();

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return Ok(new { message = $"Usuario con ID {id} eliminado correctamente." });
    }
}
