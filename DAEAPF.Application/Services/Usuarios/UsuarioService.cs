using DAEAPF.Application.DTOs.Usuarios;
using DAEAPF.Application.Interfaces.Repositories;
using DAEAPF.Application.Interfaces.Services.Usuarios;
using DAEAPF.Infrastructure.Encryptor;
using DAEAPF.Infrastructure.Repositories;

namespace DAEAPF.Application.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IPasswordHasher _hasher;

        public UsuarioService(IUsuarioRepository repository, IPasswordHasher hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var usuarios = await _repository.GetAllAsync();
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
            var usuario = await _repository.GetByIdAsync(id)
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
            var usuario = await _repository.GetByIdAsync(id)
                           ?? throw new Exception("Usuario no encontrado.");

            if (requesterRole != "admin" && requesterId != id)
                throw new UnauthorizedAccessException("No autorizado para actualizar este usuario.");

            if (dto.Rol != "cliente" && dto.Rol != "negocio")
                throw new Exception("Rol inválido. Solo 'cliente' o 'negocio'.");

            if (requesterRole == "admin" || usuario.Id == requesterId)
                usuario.Rol = dto.Rol;

            usuario.Nombre = dto.Nombre;
            usuario.Correo = dto.Correo;

            await _repository.UpdateAsync(usuario);

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

            var usuario = await _repository.GetByIdAsync(id)
                           ?? throw new Exception("Usuario no encontrado.");

            if (!_hasher.Verify(dto.CurrentPassword, usuario.ContrasenaHash))
                throw new Exception("La contraseña actual es incorrecta.");

            usuario.ContrasenaHash = _hasher.Hash(dto.NewPassword);

            await _repository.UpdateAsync(usuario);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
