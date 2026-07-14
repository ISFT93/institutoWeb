using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Application.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // Acevedo Cecilia
    public class AlumnosCarrerasModeloControllers : ControllerBase
    {
        private readonly IAlumnosCarrerasService _service;

        // Inyectamos el servicio que creamos en el paso anterior 
        public AlumnosCarrerasModeloControllers(IAlumnosCarrerasService service)
        {
            _service = service;
        }

        // GET ALL: Trae la lista completa de todas las inscripciones que existen en la tabla AlumnosCarreras
        [HttpGet]
        public async Task<ActionResult<List<AlumnosCarreras>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var lista = await _service.GetAllAsync(cancellationToken);
                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno al obtener los registros: {ex.Message}");
            }
        }

        // GET BY ID:Trae los datos de una sola fila específica usando su ID.
        // Por ejemplo, si llamás a /AlumnosCarreras/5, el GET te devuelve solo la ficha de la inscripción número 5.
        [HttpGet("{id}")]
        public async Task<ActionResult<AlumnosCarreras>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var registro = await _service.GetByIdAsync(id, cancellationToken);
                if (registro == null)
                {
                    return NotFound($"No se encontró la relación Alumno-Carrera con ID {id}"); // Devuelve HTTP 404
                }
                return Ok(registro);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno al buscar el registro: {ex.Message}");
            }
        }

        // POST (CREAR): Se usa para enviar datos nuevos al servidor con el fin de crear un registro que antes no existía.
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] AlumnosCarreras alumnoCarrera, CancellationToken cancellationToken)
        {
            try
            {
                if (alumnoCarrera == null) return BadRequest("Los datos enviados son nulos.");

                var nuevoId = await _service.CreateAsync(alumnoCarrera, cancellationToken);

                // Devolvemos un HTTP 201 (Created) indicando el ID del nuevo registro
                return CreatedAtAction(nameof(GetById), new { id = nuevoId }, nuevoId);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno al crear el registro: {ex.Message}");
            }
        }

        // PUT (ACTUALIZAR): Lo usamos para editar o reemplazar la información de un registro que ya existe en la base de datos.
        [HttpPut]
        public async Task<ActionResult<bool>> Update([FromBody] AlumnosCarreras alumnoCarrera, CancellationToken cancellationToken)
        {
            try
            {
                if (alumnoCarrera == null) return BadRequest("Los datos enviados son nulos.");

                var actualizado = await _service.UpdateAsync(alumnoCarrera, cancellationToken);
                if (!actualizado)
                {
                    return NotFound($"No se pudo actualizar. No existe el registro con ID {alumnoCarrera.AlumnoCarreraId}");
                }
                return Ok(actualizado);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno al actualizar el registro: {ex.Message}");
            }
        }

        // DELETE: Es para dar de baja definitiva o eliminar un registro físico de la base de datos.
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                var eliminado = await _service.DeleteAsync(id, cancellationToken);
                if (!eliminado)
                {
                    return NotFound($"No se pudo eliminar. No existe el registro con ID {id}");
                }
                return Ok(eliminado);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno al eliminar el registro: {ex.Message}");
            }
        }
    }
}