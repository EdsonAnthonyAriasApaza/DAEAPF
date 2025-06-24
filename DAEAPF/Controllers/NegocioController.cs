using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DAEAPF.Infrastructure.Context;
using DAEAPF.Domain.Models;
using System.Security.Claims;

namespace DAEAPF.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NegocioController : ControllerBase
{
    private readonly NegociosAppContext _context;

    public NegocioController(NegociosAppContext context)
    {
        _context = context;
    }

    // GET: api/Negocio
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var negocios = await _context.Negocios
            .Include(n => n.Usuario)
            .Include(n => n.Estado)
            .Include(n => n.Productos)
            .ToListAsync();

        return Ok(negocios);
    }

    // GET: api/Negocio/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var negocio = await _context.Negocios
            .Include(n => n.Usuario)
            .Include(n => n.Estado)
            .Include(n => n.Productos)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (negocio == null)
            return NotFound();

        return Ok(negocio);
    }

    // POST: api/Negocio
    [HttpPost]
    [Authorize(Roles = "negocio,admin")]
    public async Task<IActionResult> Create([FromBody] Negocio negocio)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        negocio.UsuarioId = int.Parse(userId);
        negocio.FechaCreacion = DateTime.UtcNow;

        _context.Negocios.Add(negocio);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = negocio.Id }, negocio);
    }

    // PUT: api/Negocio/5
    [HttpPut("{id}")]
    [Authorize(Roles = "negocio,admin")]
    public async Task<IActionResult> Update(int id, [FromBody] Negocio negocio)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        var negocioExistente = await _context.Negocios.FindAsync(id);
        if (negocioExistente == null)
            return NotFound();

        if (userRole != "admin" && negocioExistente.UsuarioId.ToString() != userId)
            return Forbid();

        // Solo actualizamos los campos permitidos
        negocioExistente.Nombre = negocio.Nombre;
        negocioExistente.Descripcion = negocio.Descripcion;
        negocioExistente.Direccion = negocio.Direccion;
        negocioExistente.Telefono = negocio.Telefono;
        negocioExistente.Categoria = negocio.Categoria;
        negocioExistente.EstadoId = negocio.EstadoId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Negocio/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "negocio,admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        var negocio = await _context.Negocios.FindAsync(id);
        if (negocio == null)
            return NotFound();

        if (userRole != "admin" && negocio.UsuarioId.ToString() != userId)
            return Forbid();

        _context.Negocios.Remove(negocio);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
