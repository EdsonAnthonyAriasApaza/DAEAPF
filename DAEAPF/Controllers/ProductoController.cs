using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DAEAPF.Infrastructure.Context;
using DAEAPF.Domain.Models;
using System.Security.Claims;

namespace DAEAPF.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductoController : ControllerBase
{
    private readonly NegociosAppContext _context;

    public ProductoController(NegociosAppContext context)
    {
        _context = context;
    }

    // GET: api/Producto
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .Include(p => p.Estado)
            .Include(p => p.Negocio)
            .ToListAsync();

        return Ok(productos);
    }

    // GET: api/Producto/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .Include(p => p.Estado)
            .Include(p => p.Negocio)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    // POST: api/Producto
    [HttpPost]
    [Authorize(Roles = "negocio,admin")]
    public async Task<IActionResult> Create([FromBody] Producto producto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        var negocio = await _context.Negocios.FindAsync(producto.NegocioId);
        if (negocio == null)
            return BadRequest("Negocio no válido.");

        if (userRole != "admin" && negocio.UsuarioId.ToString() != userId)
            return Forbid();

        producto.FechaCreacion = DateTime.UtcNow;

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
    }

    // PUT: api/Producto/5
    [HttpPut("{id}")]
    [Authorize(Roles = "negocio,admin")]
    public async Task<IActionResult> Update(int id, [FromBody] Producto producto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        var productoExistente = await _context.Productos
            .Include(p => p.Negocio)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (productoExistente == null)
            return NotFound();

        if (userRole != "admin" && productoExistente.Negocio.UsuarioId.ToString() != userId)
            return Forbid();

        productoExistente.Nombre = producto.Nombre;
        productoExistente.Descripcion = producto.Descripcion;
        productoExistente.Precio = producto.Precio;
        productoExistente.CategoriaId = producto.CategoriaId;
        productoExistente.EstadoId = producto.EstadoId;
        productoExistente.ImagenUrl = producto.ImagenUrl;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Producto/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "negocio,admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        var producto = await _context.Productos
            .Include(p => p.Negocio)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
            return NotFound();

        if (userRole != "admin" && producto.Negocio.UsuarioId.ToString() != userId)
            return Forbid();

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
