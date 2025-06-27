using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DAEAPF.Application.DTOs.Productos;
using DAEAPF.Application.Interfaces.Services.Productos;

namespace DAEAPF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        /// <summary>
        /// GET: api/Producto/Negocio/{negocioId}
        /// Listar productos de un negocio específico (público)
        /// </summary>
        [HttpGet("Negocio/{negocioId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByNegocio(int negocioId)
        {
            var productos = await _productoService.GetByNegocioIdAsync(negocioId);
            return Ok(productos);
        }

        /// <summary>
        /// POST: api/Producto
        /// Crear producto (solo dueño o admin)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "negocio,admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            try
            {
                var result = await _productoService.CreateAsync(dto, userId, userRole);
                return CreatedAtAction(nameof(GetByNegocio), new { negocioId = result.NegocioId }, result);
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
        /// PUT: api/Producto/{id}
        /// Actualizar producto (solo dueño o admin)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "negocio,admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            try
            {
                var result = await _productoService.UpdateAsync(id, dto, userId, userRole);
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
        /// DELETE: api/Producto/{id}
        /// Eliminar producto (solo dueño o admin)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "negocio,admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            try
            {
                await _productoService.DeleteAsync(id, userId, userRole);
                return Ok(new { message = "Producto eliminado exitosamente." });
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
