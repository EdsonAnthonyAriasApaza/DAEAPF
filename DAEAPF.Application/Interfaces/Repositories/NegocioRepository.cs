using DAEAPF.Application.Interfaces.Repositories;
using DAEAPF.Domain.Models;
using DAEAPF.Infrastructure.Context;
using DAEAPF.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAEAPF.Application.Interfaces.Repositories
{
    public class NegocioRepository : INegocioRepository
    {
        private readonly NegociosAppContext _context;

        public NegocioRepository(NegociosAppContext context)
        {
            _context = context;
        }

        public IQueryable<Negocio> Query()
        {
            return _context.Negocios
                .Include(n => n.Estado)
                .Include(n => n.Usuario)
                .Include(n => n.Productos)
                .AsQueryable();
        }

        public async Task<IEnumerable<Negocio>> GetAllAsync()
        {
            return await _context.Negocios
                .Include(n => n.Estado)
                .Include(n => n.Usuario)
                .Include(n => n.Productos)
                .ToListAsync();
        }

        public async Task<Negocio?> GetByIdAsync(int id)
        {
            return await _context.Negocios
                .Include(n => n.Estado)
                .Include(n => n.Usuario)
                .Include(n => n.Productos)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task AddAsync(Negocio negocio)
        {
            _context.Negocios.Add(negocio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Negocio negocio)
        {
            _context.Negocios.Update(negocio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var negocio = await _context.Negocios.FindAsync(id);
            if (negocio != null)
            {
                _context.Negocios.Remove(negocio);
                await _context.SaveChangesAsync();
            }
        }
    }
}