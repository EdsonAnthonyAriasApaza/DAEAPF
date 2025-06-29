using DAEAPF.Application.DTOs;
using DAEAPF.Application.DTOs.Usuarios;
using DAEAPF.Application.Interfaces.Services.Usuarios;
using DAEAPF.Domain.Models;
using DAEAPF.Infrastructure.Encryptor;
using DAEAPF.Infrastructure.JWT;
using DAEAPF.Infrastructure.Repositories;

namespace DAEAPF.Application.Services.Usuarios
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtService _jwtService;

        public AuthService(IAuthRepository authRepository, IPasswordHasher hasher, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _hasher = hasher;
            _jwtService = jwtService;
        }

        public async Task RegisterAsync(RegisterUserDto dto)
        {
            // Validar email único
            if (await _authRepository.EmailExistsAsync(dto.Correo))
                throw new Exception("Este correo ya está registrado.");

            // Validar rol permitido
            if (dto.Rol != "cliente" && dto.Rol != "negocio")
                throw new Exception("Rol inválido. Debe ser 'cliente' o 'negocio'.");

            var user = new Usuario
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Rol = dto.Rol,
                ContrasenaHash = _hasher.Hash(dto.Contrasena)
            };

            await _authRepository.AddAsync(user);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginUserDto dto)
        {
            var user = await _authRepository.GetByEmailAsync(dto.Correo);

            if (user == null || !_hasher.Verify(dto.Contrasena, user.ContrasenaHash))
                throw new UnauthorizedAccessException("Correo o contraseña incorrectos.");

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Correo, user.Rol);

            return new LoginResponseDto
            {
                Token = token,
                Id = user.Id,
                Nombre = user.Nombre,
                Correo = user.Correo,
                Rol = user.Rol
            };
        }
    }
}
