using instituto93.Application;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;
using instituto93.Application.Interfaces;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InscripcionMateriaController : ControllerBase
    {
        private readonly IInscripcionMateriaService _service;

        public InscripcionMateriaController(IInscripcionMateriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<InscripcionMateria>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var inscripcionMateria = await _service.GetInscripcionMateriaAsync(cancellationToken);
                return Ok(inscripcionMateria);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
