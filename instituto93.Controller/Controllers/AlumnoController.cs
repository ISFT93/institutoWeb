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
    public class AlumnoController : ControllerBase
    {
        private readonly IAlumnoService _service; // <- usar la interfaz

        public AlumnoController(IAlumnoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlumnoModelo>>> Get(CancellationToken cancellationToken)
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
        [HttpPost]
        public async Task<ActionResult<int>> CreateAlumno([FromBody] AlumnoModelo alumno, CancellationToken cancellationToken)
        {
            try
            {
                var alumnoId = await _service.CreateAlumnoModelo(alumno, cancellationToken);
                return Ok(alumnoId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}