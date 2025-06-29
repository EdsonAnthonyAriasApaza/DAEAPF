using DAEAPF.Application.Interfaces.Repositories;
using DAEAPF.Domain.Models;
using DAEAPF.Infrastructure.Context;
using DAEAPF.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAEAPF.Application.Interfaces.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly NegociosAppContext _context;

        public ProductoRepository(NegociosAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetByNegocioIdAsync(int negocioId)
        {
            return await _context.Productos
                .Include(p => p.Negocio)
                .Where(p => p.NegocioId == negocioId)
                .ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Negocio)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}