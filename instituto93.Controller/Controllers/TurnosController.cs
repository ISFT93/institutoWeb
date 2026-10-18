using instituto93.Application.Interfaces;
using instituto93.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly ITurnosService _service;
        public TurnosController(ITurnosService service)
        {
            _service = service;
        }

        [HttpGet("lookup")]
        public async Task<ActionResult<List<TurnosLookup>>> GetLookup(CancellationToken cancellationToken)
        {
            try
            {
                var turnos = await _service.GetLookupAsync(cancellationToken);
                return Ok(turnos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "No se pudieron obtener los turnos.",
                        detail = ex.Message
                    });
            }
        }
    }
}
