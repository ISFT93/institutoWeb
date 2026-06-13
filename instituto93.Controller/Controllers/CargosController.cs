using instituto93.Application;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CargosController : ControllerBase
    {
        private readonly ICargosService _service; // <- usar la interfaz

        public CargosController(ICargosService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CargosModelo>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var cargos = await _service.GetCargos(cancellationToken);
                return Ok(cargos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}