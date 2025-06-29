using DAEAPF.Application.DTOs.Productos;
using DAEAPF.Application.Interfaces.Services.Productos;
using DAEAPF.Domain.Models;
using DAEAPF.Infrastructure.Repositories;

namespace DAEAPF.Application.Services.Productos
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly INegocioRepository _negocioRepository;

        public ProductoService(IProductoRepository productoRepository, INegocioRepository negocioRepository)
        {
            _productoRepository = productoRepository;
            _negocioRepository = negocioRepository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetByNegocioIdAsync(int negocioId)
        {
            var productos = await _productoRepository.GetByNegocioIdAsync(negocioId);
            return productos.Select(MapToResponseDto).ToList();
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto, int userId, string userRole)
        {
            var negocio = await _negocioRepository.GetByIdAsync(dto.NegocioId);
            if (negocio == null)
                throw new Exception("Negocio no encontrado.");

            if (userRole != "admin" && negocio.UsuarioId != userId)
                throw new UnauthorizedAccessException();

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                NegocioId = dto.NegocioId
            };

            await _productoRepository.AddAsync(producto);

            return MapToResponseDto(producto);
        }

        public async Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto, int userId, string userRole)
        {
            var producto = await _productoRepository.GetByIdAsync(id);

            if (producto == null)
                throw new Exception("Producto no encontrado.");

            if (userRole != "admin" && producto.Negocio.UsuarioId != userId)
                throw new UnauthorizedAccessException();

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;

            await _productoRepository.UpdateAsync(producto);

            return MapToResponseDto(producto);
        }

        public async Task DeleteAsync(int id, int userId, string userRole)
        {
            var producto = await _productoRepository.GetByIdAsync(id);

            if (producto == null)
                throw new Exception("Producto no encontrado.");

            if (userRole != "admin" && producto.Negocio.UsuarioId != userId)
                throw new UnauthorizedAccessException();

            await _productoRepository.DeleteAsync(id);
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
