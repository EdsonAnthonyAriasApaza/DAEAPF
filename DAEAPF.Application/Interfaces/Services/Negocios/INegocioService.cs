using DAEAPF.Application.DTOs.Negocios;

namespace DAEAPF.Application.Interfaces.Services.Negocios
{
    public interface INegocioService
    {
        Task<IEnumerable<NegocioResponseDto>> GetAllAsync(NegocioFilterDto filter);
        Task<NegocioResponseDto> GetByIdAsync(int id);
        Task<NegocioResponseDto> CreateAsync(CreateNegocioDto dto, int ownerId);
        Task<NegocioResponseDto> UpdateAsync(int id, UpdateNegocioDto dto, int requesterId);
        Task DeleteAsync(int id, int requesterId);
    }
}