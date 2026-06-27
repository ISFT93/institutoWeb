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
        private readonly IParametrosService _service;

        public ParametrosController(IParametrosService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Parametros>>> Get(CancellationToken cancellationToken)
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
