using instituto93.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Application;
using instituto93.Application.Interfaces;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CursoMateriasController : ControllerBase
    {
        private readonly ICursoMateriasService _service;

        public CursoMateriasController(ICursoMateriasService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CursoMaterias>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var cursoMaterias = await _service.GetCursoMaterias(cancellationToken);
                return Ok(cursoMaterias);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
