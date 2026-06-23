using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Application;
using instituto93.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CicloLectivoController : ControllerBase
    {
        private readonly ICicloLectivoService _service;

        public CicloLectivoController(ICicloLectivoService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // 1. Obtener todos los ciclos lectivos (GET /CicloLectivo)
        [HttpGet]
        public async Task<ActionResult<List<CicloLectivoModelo>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var ciclos = await _service.GetCiclosLectivosAsync(cancellationToken);
                return Ok(ciclos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 2. Obtener un ciclo por año (GET /CicloLectivo/2026)
        [HttpGet("{anioLectivo}")]
        public async Task<ActionResult<CicloLectivoModelo>> GetByAnio(int anioLectivo, CancellationToken cancellationToken)
        {
            try
            {
                var ciclo = await _service.GetByAnioAsync(anioLectivo, cancellationToken);
                if (ciclo == null)
                {
                    return NotFound($"No se encontró el ciclo lectivo para el año {anioLectivo}");
                }
                return Ok(ciclo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 3. Crear un nuevo ciclo (POST /CicloLectivo)
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken)
        {
            try
            {
                var nuevoAnio = await _service.CreateAsync(cicloLectivo, cancellationToken);
                return Ok(nuevoAnio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 4. Actualizar un ciclo existente (PUT /CicloLectivo)
        [HttpPut]
        public async Task<ActionResult<bool>> Update([FromBody] CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken)
        {
            try
            {
                var actualizado = await _service.UpdateAsync(cicloLectivo, cancellationToken);
                if (!actualizado)
                {
                    return NotFound($"No se pudo actualizar. Es posible que el año {cicloLectivo.AnioLectivo} no exista.");
                }
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 5. Borrar un ciclo por año (DELETE /CicloLectivo/2026)
        [HttpDelete("{anioLectivo}")]
        public async Task<ActionResult<bool>> Delete(int anioLectivo, CancellationToken cancellationToken)
        {
            try
            {
                var eliminado = await _service.DeleteAsync(anioLectivo, cancellationToken);
                if (!eliminado)
                {
                    return NotFound($"No se pudo eliminar. Es posible que el año {anioLectivo} no exista.");
                }
                return Ok(eliminado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}