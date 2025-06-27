using DAEAPF.Application.DTOs.Productos;
using DAEAPF.Application.Interfaces.Services;
using DAEAPF.Application.Interfaces.Services.Productos;
using DAEAPF.Domain.Models;
using DAEAPF.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DAEAPF.Application.Services.Productos
{
    public class ProductoService : IProductoService
    {
        private readonly NegociosAppContext _context;

        public ProductoService(NegociosAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetByNegocioIdAsync(int negocioId)
        {
            var productos = await _context.Productos
                .Where(p => p.NegocioId == negocioId)
                .Include(p => p.Negocio)
                .ToListAsync();

            return productos.Select(MapToResponseDto).ToList();
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto, int userId, string userRole)
        {
            // Verificar que el negocio exista
            var negocio = await _context.Negocios.FindAsync(dto.NegocioId);
            if (negocio == null)
                throw new Exception("Negocio no encontrado.");

            // Solo dueño del negocio o admin puede crear
            if (userRole != "admin" && negocio.UsuarioId != userId)
                throw new UnauthorizedAccessException();

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                NegocioId = dto.NegocioId
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return MapToResponseDto(producto);
        }

        public async Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto, int userId, string userRole)
        {
            var producto = await _context.Productos
                .Include(p => p.Negocio)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                throw new Exception("Producto no encontrado.");

            // Validar que sea dueño o admin
            if (userRole != "admin" && producto.Negocio.UsuarioId != userId)
                throw new UnauthorizedAccessException();

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;

            await _context.SaveChangesAsync();

            return MapToResponseDto(producto);
        }

        public async Task DeleteAsync(int id, int userId, string userRole)
        {
            var producto = await _context.Productos
                .Include(p => p.Negocio)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                throw new Exception("Producto no encontrado.");

            // Validar que sea dueño o admin
            if (userRole != "admin" && producto.Negocio.UsuarioId != userId)
                throw new UnauthorizedAccessException();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
        }

        // 🔁 Mapping interno
        private ProductResponseDto MapToResponseDto(Producto producto)
        {
            return new ProductResponseDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                NegocioId = producto.NegocioId,
                NegocioNombre = producto.Negocio?.Nombre
            };
        }
    }
}
