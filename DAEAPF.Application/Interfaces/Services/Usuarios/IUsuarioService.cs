using DAEAPF.Application.DTOs.Usuarios;

namespace DAEAPF.Application.Interfaces.Services.Usuarios;

public interface IUsuarioService
{
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto> GetByIdAsync(int id);
    Task<UserResponseDto> UpdateAsync(int id, UpdateUserDto dto, int requesterId, string requesterRole);
    Task ChangePasswordAsync(int id, ChangePasswordDto dto, int requesterId);
    Task DeleteAsync(int id);
}