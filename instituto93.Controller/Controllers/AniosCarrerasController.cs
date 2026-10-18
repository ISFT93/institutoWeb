using instituto93.Application.Interfaces;
using instituto93.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AniosCarrerasController : ControllerBase
    {
        private readonly IAniosCarrerasService _service;
        public AniosCarrerasController(IAniosCarrerasService service)
        {
            _service = service;
        }

        [HttpGet("lookup")]
        public async Task<ActionResult<List<AnioCarreraLookup>>> GetLookup([FromQuery] int carreraId, CancellationToken cancellationToken)
        {
            if (carreraId <= 0)
                return BadRequest(new { message = "El id de la carrera es obligatorio." });

            try
            {
                var anios = await _service.GetLookupAsync(carreraId, cancellationToken);
                return Ok(anios);
            }
            catch (System.Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "No se pudieron obtener los años de la carrera.",
                        detail = ex.Message
                    });
            }
        }
    }
}
