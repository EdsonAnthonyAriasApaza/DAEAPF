using DAEAPF.Application.DTOs;
using DAEAPF.Application.DTOs.Usuarios;
using DAEAPF.Application.Interfaces.Services.Usuarios;
using DAEAPF.Domain.Models;
using DAEAPF.Infrastructure.Context;
using DAEAPF.Infrastructure.Encryptor;
using DAEAPF.Infrastructure.JWT;
using Microsoft.EntityFrameworkCore;

namespace DAEAPF.Application.Services.Usuarios
{
    public class AuthService : IAuthService
    {
        private readonly NegociosAppContext _context;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtService _jwtService;

        public AuthService(NegociosAppContext context, IPasswordHasher hasher, IJwtService jwtService)
        {
            _context = context;
            _hasher = hasher;
            _jwtService = jwtService;
        }

        public async Task RegisterAsync(RegisterUserDto dto)
        {
            // Validar email único
            if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
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

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<LoginResponseDto> LoginAsync(LoginUserDto dto)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == dto.Correo);

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
