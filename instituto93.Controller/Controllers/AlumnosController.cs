using instituto93.Application;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;
//Lopez Melany
namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlumnosController : ControllerBase
    {
        private readonly IAlumnosService _service; // <- usar la interfaz

        public AlumnosController(IAlumnosService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlumnosModelo>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var alumnosModelos = await _service.GetAlumnosModelos(cancellationToken);
                return Ok(alumnosModelos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}