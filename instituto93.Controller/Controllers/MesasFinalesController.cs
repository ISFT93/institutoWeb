using instituto93.Application;
using instituto93.Application.Interfaces;
using instituto93.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MesasFinalesController : ControllerBase
    {
        private readonly IMesasFinalesService _service;
        private readonly IMesaAlumnoService _mesaAlumnoService;
        public MesasFinalesController(IMesasFinalesService service, IMesaAlumnoService mesaAlumnoService)
        {
            _service = service;
            _mesaAlumnoService = mesaAlumnoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MesaFinalFila>>> GetList(
        [FromQuery] int? carreraId,
        [FromQuery] int? turnoId,
        [FromQuery] int? anioCarreraId,
        [FromQuery] int? anioLectivo,
        [FromQuery] string? texto,
        CancellationToken cancellationToken)
        {
            int? alumnoId = null;
            var alumnoIdClaim = User.FindFirstValue(AuthTokenService.AlumnoIdClaim);
            if (int.TryParse(alumnoIdClaim, out var alumnoIdParsed))
                alumnoId = alumnoIdParsed;

            var filtro = new MesasFinalesFiltro(carreraId, turnoId, anioCarreraId, anioLectivo, texto, alumnoId);

            try
            {
                var mesas = await _service.GetListAsync(filtro, cancellationToken);
                return Ok(mesas);
            }
            catch (System.Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "No se pudieron obtener las mesas de finales.",
                        detail = ex.Message
                    });
            }
        }

        [HttpPost("inscribir")]
        public async Task<IActionResult> Inscribir([FromBody] List<int> mesaFinalIds, CancellationToken cancellationToken)
        {
            var alumnoIdClaim = User.FindFirstValue(AuthTokenService.AlumnoIdClaim);
            if (!int.TryParse(alumnoIdClaim, out var alumnoId) || alumnoId <= 0)
                return Forbid();

            if (mesaFinalIds == null || mesaFinalIds.Count == 0)
                return BadRequest(new { message = "Debe seleccionar al menos una mesa." });

            try
            {
                var filas = await _mesaAlumnoService.InscribirAsync(alumnoId, mesaFinalIds, cancellationToken);
                return Ok(new { inscriptas = filas });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "No se pudo completar la inscripción.", detail = ex.Message });
            }
        }
    }
}
