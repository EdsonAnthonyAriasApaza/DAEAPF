using DAEAPF.Application.DTOs.Usuarios;
using DAEAPF.Application.Interfaces.Services.Usuarios;
using DAEAPF.Infrastructure.Context;
using DAEAPF.Infrastructure.Encryptor;
using Microsoft.EntityFrameworkCore;

namespace DAEAPF.Application.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly NegociosAppContext _context;
        private readonly IPasswordHasher _hasher;

        public UsuarioService(NegociosAppContext context, IPasswordHasher hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return usuarios.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Correo = u.Correo,
                Rol = u.Rol,
                FechaRegistro = u.FechaRegistro
            });
        }

        public async Task<UserResponseDto> GetByIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id)
                           ?? throw new Exception("Usuario no encontrado.");

            return new UserResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
        }

        public async Task<UserResponseDto> UpdateAsync(int id, UpdateUserDto dto, int requesterId, string requesterRole)
        {
            var usuario = await _context.Usuarios.FindAsync(id)
                           ?? throw new Exception("Usuario no encontrado.");

            // Permiso: admin o dueño de su cuenta
            if (requesterRole != "admin" && requesterId != id)
                throw new UnauthorizedAccessException("No autorizado para actualizar este usuario.");

            // Validar rol permitido
            if (dto.Rol != "cliente" && dto.Rol != "negocio")
                throw new Exception("Rol inválido. Solo 'cliente' o 'negocio'.");

            // Solo admin puede cambiar el rol
            if (requesterRole == "admin" || usuario.Id == requesterId)
                usuario.Rol = dto.Rol;

            usuario.Nombre = dto.Nombre;
            usuario.Correo = dto.Correo;

            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
        }

        public async Task ChangePasswordAsync(int id, ChangePasswordDto dto, int requesterId)
        {
            if (requesterId != id)
                throw new UnauthorizedAccessException("No autorizado para cambiar la contraseña de otro usuario.");

            var usuario = await _context.Usuarios.FindAsync(id)
                           ?? throw new Exception("Usuario no encontrado.");

            // Verificar contraseña actual
            if (!_hasher.Verify(dto.CurrentPassword, usuario.ContrasenaHash))
                throw new Exception("La contraseña actual es incorrecta.");

            // Asignar nueva contraseña hasheada
            usuario.ContrasenaHash = _hasher.Hash(dto.NewPassword);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id)
                           ?? throw new Exception("Usuario no encontrado.");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
