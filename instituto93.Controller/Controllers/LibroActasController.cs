using instituto93.Application;
using instituto93.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using instituto93.Application.Interfaces;

namespace instituto93.Controller.Controllers
{
    //Ibarra Valentino
    [ApiController]
    [Route("[controller]")]
    public class LibroActasController : ControllerBase
    {
        private readonly ILibroActasService _service; // <- usar la interfaz

        public LibroActasController(ILibroActasService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<LibroActasModelo>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var libroActas = await _service.GetLibroActas(cancellationToken);
                return Ok(libroActas);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
