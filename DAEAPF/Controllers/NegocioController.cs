using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAEAPF.Application.DTOs.Negocios;
using DAEAPF.Application.Interfaces.Services.Negocios;

namespace DAEAPF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NegocioController : ControllerBase
    {
        private readonly INegocioService _negocioService;

        public NegocioController(INegocioService negocioService)
        {
            _negocioService = negocioService;
        }

        /// <summary>
        /// Obtener todos los negocios con filtros opcionales
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] NegocioFilterDto filter)
        {
            var result = await _negocioService.GetAllAsync(filter);
            return Ok(result);
        }

        /// <summary>
        /// Obtener detalle de un negocio por ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _negocioService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crear un nuevo negocio (solo dueños autenticados)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "negocio,admin")]
        public async Task<IActionResult> Create([FromBody] CreateNegocioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var result = await _negocioService.CreateAsync(dto, ownerId);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar información del negocio (solo dueño o admin)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "negocio,admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNegocioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var requesterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var result = await _negocioService.UpdateAsync(id, dto, requesterId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Eliminar negocio (solo dueño o admin)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "negocio,admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var requesterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                await _negocioService.DeleteAsync(id, requesterId);
                return Ok(new { message = "Negocio eliminado exitosamente." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
