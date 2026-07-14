using instituto93.Application;
using instituto93.Domain.Interfaces;
using instituto93.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace instituto93.Controller.Controllers
{
    //Mourdad ignacio
    [ApiController]
    [Route("[controller]")]
    public class HorarioController : ControllerBase
    {
        private readonly IHorarioService _service;

        public HorarioController(IHorarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<IHorario>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var horarios = await _service.GetHorarios(cancellationToken);
                return Ok(horarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<IHorario>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var horario = await _service.GetHorarioById(id, cancellationToken);

                if (horario == null)
                    return NotFound("No se encontró el horario solicitado.");

                return Ok(horario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("curso-materia/{cursoMateriaId:int}")]
        public async Task<ActionResult<List<IHorario>>> GetByCursoMateria(int cursoMateriaId, CancellationToken cancellationToken)
        {
            try
            {
                var horarios = await _service.GetHorariosByCursoMateria(cursoMateriaId, cancellationToken);
                return Ok(horarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] IHorario horario, CancellationToken cancellationToken)
        {
            try
            {
                var nuevoId = await _service.CrearHorario(horario, cancellationToken);

                return Ok(new
                {
                    mensaje = "Horario creado correctamente.",
                    horarioId = nuevoId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] IHorario horario, CancellationToken cancellationToken)
        {
            try
            {
                var actualizado = await _service.ActualizarHorario(id, horario, cancellationToken);

                if (!actualizado)
                    return NotFound("No se encontró el horario que desea modificar.");

                return Ok("Horario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                var eliminado = await _service.EliminarHorario(id, cancellationToken);

                if (!eliminado)
                    return NotFound("No se encontró el horario que desea eliminar.");

                return Ok("Horario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
