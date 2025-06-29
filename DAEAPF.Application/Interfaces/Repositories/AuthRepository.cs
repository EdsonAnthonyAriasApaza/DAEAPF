using DAEAPF.Application.Interfaces.Repositories;
using DAEAPF.Domain.Models;
using DAEAPF.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DAEAPF.Infrastructure.Repositories;

namespace DAEAPF.Application.Interfaces.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly NegociosAppContext _context;

        public AuthRepository(NegociosAppContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Correo == email);
        }

        public async Task AddAsync(Usuario user)
        {
            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}