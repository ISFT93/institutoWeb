using instituto93.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using instituto93.Domain.Models;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaisController : ControllerBase
    {
        private readonly IPaisService _service; // <- usar la interfaz

        public PaisController(IPaisService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Pais>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var paises = await _service.GetPaisAsync(cancellationToken);
                return Ok(paises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pais>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var pais = await _service.GetPaisByIdAsync(id, cancellationToken);

                if (pais is null) return NotFound($"No se encontró el país con el id {id}");

                return Ok(pais);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreatePais([FromBody] Pais pais, CancellationToken cancellationToken)
        {
            try
            {
                var paisId = await _service.CreatePaisAsync(pais, cancellationToken);
                return Ok(paisId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdatePais([FromBody] Pais pais, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _service.UpdatePaisAsync(pais, cancellationToken);

                if (!res) return NotFound($"No se encontró un país con el id {pais.Id}");

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeletePais(int id, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _service.DeletePaisAsync(id, cancellationToken);

                if (!res) return NotFound($"No se encontró el país con el id {id}");

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}