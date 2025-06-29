using DAEAPF.Domain.Models;

namespace DAEAPF.Infrastructure.Repositories
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetByNegocioIdAsync(int negocioId);
        Task<Producto?> GetByIdAsync(int id);
        Task AddAsync(Producto producto);
        Task UpdateAsync(Producto producto);
        Task DeleteAsync(int id);
    }
}