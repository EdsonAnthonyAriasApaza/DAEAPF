using DAEAPF.Application.DTOs;
using DAEAPF.Application.DTOs.Usuarios;

namespace DAEAPF.Application.Interfaces.Services.Usuarios
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterUserDto dto);
        Task<LoginResponseDto> LoginAsync(LoginUserDto dto);
    }
}