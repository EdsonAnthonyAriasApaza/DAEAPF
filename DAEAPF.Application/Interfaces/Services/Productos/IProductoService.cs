using DAEAPF.Application.DTOs.Productos;

namespace DAEAPF.Application.Interfaces.Services.Productos
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductResponseDto>> GetByNegocioIdAsync(int negocioId);

        Task<ProductResponseDto> CreateAsync(CreateProductDto dto, int userId, string userRole);

        Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto, int userId, string userRole);

        Task DeleteAsync(int id, int userId, string userRole);
    }
}