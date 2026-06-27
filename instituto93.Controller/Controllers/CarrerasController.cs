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
    public class CarrerasController : ControllerBase
    {
        private readonly ICarrerasService _service; // <- usar la interfaz
        public CarrerasController(ICarrerasService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CarrerasModelo>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var carreras = await _service.GetCarreras(cancellationToken);
                return Ok(carreras);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
