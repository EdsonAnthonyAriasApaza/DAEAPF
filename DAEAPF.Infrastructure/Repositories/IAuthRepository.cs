using DAEAPF.Domain.Models;

namespace DAEAPF.Infrastructure.Repositories;

public interface IAuthRepository
{
    Task<Usuario> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task AddAsync(Usuario user);
}