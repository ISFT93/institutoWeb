using instituto93.Application;
using instituto93.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using instituto93.Application.Interfaces;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MateriasModeloController : ControllerBase
    {
        private readonly IMateriasModeloService _service; // <- usar la interfaz

        public MateriasModeloController(IMateriasModeloService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MateriasModelo>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var materiasModelo = await _service.GetMateriasModelo(cancellationToken);
                return Ok(materiasModelo);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
