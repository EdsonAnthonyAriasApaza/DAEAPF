using DAEAPF.Domain.Models;

namespace DAEAPF.Infrastructure.Repositories
{
    public interface INegocioRepository
    {
        Task<IEnumerable<Negocio>> GetAllAsync();
        Task<Negocio?> GetByIdAsync(int id);
        Task AddAsync(Negocio negocio);
        Task UpdateAsync(Negocio negocio);
        Task DeleteAsync(int id);
        IQueryable<Negocio> Query(); // Para soportar filtros dinámicos
    }
}