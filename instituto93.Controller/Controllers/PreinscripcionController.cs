using instituto93.Application;
using instituto93.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class PreinscripcionController : ControllerBase
    {
        private readonly ICicloLectivoService _ciclos;

        public PreinscripcionController(ICicloLectivoService ciclos)
        {
            _ciclos = ciclos;
        }

        [HttpGet("estado")]
        public async Task<ActionResult<EstadoPreinscripcionDto>> GetEstado(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _ciclos.GetEstadoPreinscripcionAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "No se pudo consultar el plazo de preinscripción.", detail = ex.Message });
            }
        }
    }
}
