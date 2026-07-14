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
    public class ParametrosController : ControllerBase
    {
        private readonly IParametroService _service;

        public ParametrosController(IParametroService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Parametro>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var parametros = await _service.GetParametrosAsync(cancellationToken);
                return Ok(parametros);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
