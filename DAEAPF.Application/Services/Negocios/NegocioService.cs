using DAEAPF.Application.DTOs.Negocios;
using DAEAPF.Application.Interfaces.Services.Negocios;
using DAEAPF.Domain.Models;
using DAEAPF.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace DAEAPF.Application.Services.Negocios
{
    public class NegocioService : INegocioService
    {
        private readonly NegociosAppContext _context;

        public NegocioService(NegociosAppContext context)
        {
            _context = context;
        }

        // Obtener todos con filtros
        public async Task<IEnumerable<NegocioResponseDto>> GetAllAsync(NegocioFilterDto filter)
        {
            var query = _context.Negocios
                .Include(n => n.Estado)
                .Include(n => n.Usuario)
                .Include(n => n.Productos)
                .AsQueryable();

            // Filtro por categoría exacta
            if (!string.IsNullOrWhiteSpace(filter.Categoria))
            {
                query = query.Where(n => n.Categoria == filter.Categoria);
            }

            // Filtro por estado exacto
            if (filter.EstadoId.HasValue)
            {
                query = query.Where(n => n.EstadoId == filter.EstadoId.Value);
            }

            // Búsqueda general en Nombre o Dirección (insensible a mayúsculas/minúsculas)
            if (!string.IsNullOrWhiteSpace(filter.NombreBusqueda))
            {
                var term = filter.NombreBusqueda.Trim().ToLower();
                query = query.Where(n =>
                    n.Nombre.ToLower().Contains(term) ||
                    n.Direccion.ToLower().Contains(term)
                );
            }

            // Paginación opcional
            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
            {
                int skip = (filter.PageNumber.Value - 1) * filter.PageSize.Value;
                query = query.Skip(skip).Take(filter.PageSize.Value);
            }

            var negocios = await query.ToListAsync();
            return negocios.Select(MapToResponseDto).ToList();
        }


        // Obtener uno por ID
        public async Task<NegocioResponseDto> GetByIdAsync(int id)
        {
            var negocio = await _context.Negocios
                .Include(n => n.Estado)
                .Include(n => n.Usuario)
                .Include(n => n.Productos)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (negocio == null)
                throw new Exception("Negocio no encontrado");

            return MapToResponseDto(negocio);
        }

        // Crear
        public async Task<NegocioResponseDto> CreateAsync(CreateNegocioDto dto, int ownerId)
        {
            var negocio = new Negocio
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                Categoria = dto.Categoria,
                UsuarioId = ownerId,
                EstadoId = dto.EstadoId ?? 1 // Por defecto abierto o inicial
            };

            _context.Negocios.Add(negocio);
            await _context.SaveChangesAsync();

            // Vuelve a cargar con includes para DTO
            return await GetByIdAsync(negocio.Id);
        }

        // Actualizar
        public async Task<NegocioResponseDto> UpdateAsync(int id, UpdateNegocioDto dto, int requesterId)
        {
            var negocio = await _context.Negocios.FirstOrDefaultAsync(n => n.Id == id);

            if (negocio == null)
                throw new Exception("Negocio no encontrado");

            // Validar dueño
            if (negocio.UsuarioId != requesterId)
                throw new UnauthorizedAccessException();

            negocio.Nombre = dto.Nombre;
            negocio.Descripcion = dto.Descripcion;
            negocio.Direccion = dto.Direccion;
            negocio.Telefono = dto.Telefono;
            negocio.Categoria = dto.Categoria;
            negocio.EstadoId = dto.EstadoId;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(negocio.Id);
        }

        // Eliminar
        public async Task DeleteAsync(int id, int requesterId)
        {
            var negocio = await _context.Negocios.FindAsync(id);

            if (negocio == null)
                throw new Exception("Negocio no encontrado");

            if (negocio.UsuarioId != requesterId)
                throw new UnauthorizedAccessException();

            _context.Negocios.Remove(negocio);
            await _context.SaveChangesAsync();
        }

        // Mapeador privado
        private NegocioResponseDto MapToResponseDto(Negocio n)
        {
            return new NegocioResponseDto
            {
                Id = n.Id,
                Nombre = n.Nombre,
                Descripcion = n.Descripcion,
                Direccion = n.Direccion,
                Telefono = n.Telefono,
                Categoria = n.Categoria,
                Estado = n.Estado?.Estado,
                FechaCreacion = n.FechaCreacion,
                DueñoId = n.UsuarioId,
                DueñoNombre = n.Usuario?.Nombre,
                Productos = n.Productos?.Select(p => new ProductoSimpleDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    ImagenUrl = p.ImagenUrl
                }).ToList()
            };
        }
    }
}