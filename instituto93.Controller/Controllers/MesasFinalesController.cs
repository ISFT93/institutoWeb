using instituto93.Application.Interfaces;
using instituto93.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MesasFinalesController : ControllerBase
    {
        private readonly IMesasFinalesService _service;
        public MesasFinalesController(IMesasFinalesService service)
        {
            _service = service;
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
            var filtro = new MesasFinalesFiltro(carreraId, turnoId, anioCarreraId, anioLectivo, texto);

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
    }
}
