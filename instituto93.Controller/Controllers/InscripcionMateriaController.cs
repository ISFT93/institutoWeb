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
        public async Task<ActionResult<List<InscripcionMateria>>> GetInscripcionMateriaAsync(CancellationToken cancellationToken)
        {
            try
            {
                var get = await _service.GetInscripcionMateriaAsync(cancellationToken);
                return Ok(get);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<List<InscripcionMateria>>> GetInscripcionMateriaByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var get = await _service.GetInscripcionMateriaByIdAsync(id, cancellationToken);
                return Ok(get);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateInscripcionMateriaAsync(int alumnoCarreraId, int cursadaId, int anioCicloLectivo, string estado,
            int horasCursada, DateTime ultimoPresentismo, decimal porcentajeAsistencia, string cursada, bool activo, CancellationToken cancellationToken = default)
        {
            try
            {
                var create = await _service.CreateInscripcionMateriaAsync(alumnoCarreraId, cursadaId, anioCicloLectivo, estado,
            horasCursada, ultimoPresentismo, porcentajeAsistencia, cursada, activo,cancellationToken);
                return Ok(create);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateInscripcionMateriaAsync(int id, InscripcionMateria inscripcionMateria, CancellationToken cancellationToken = default)
        {
            try
            {
                var update = await _service.UpdateInscripcionMateriaAsync(id, inscripcionMateria,cancellationToken);
                return Ok(update);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteInscripcionMateriaAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var delete = await _service.DeleteInscripcionMateriaAsync(id, cancellationToken);
                return Ok(delete);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
