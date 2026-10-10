using instituto93.Application;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;
using instituto93.Domain.DTOs;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlumnoController : ControllerBase
    {
        private readonly IAlumnoService _service;

        public AlumnoController(
            IAlumnoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlumnoModelo>>> Get(
            CancellationToken cancellationToken)
        {
            try
            {
                var alumnos =
                    await _service
                        .GetAlumnosModelos(
                            cancellationToken);

                return Ok(alumnos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "No se pudieron obtener los alumnos.",

                        detail =
                            ex.Message
                    });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AlumnoModelo>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var alumno =
                    await _service
                        .GetAlumnoByIdAsync(
                            id,
                            cancellationToken);

                if (alumno == null)
                    return NotFound();

                return Ok(alumno);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "No se pudo obtener el alumno.",

                        detail =
                            ex.Message
                    });
            }
        }

        [HttpPost("preinscripcion")]
        public async Task<ActionResult> CreatePreinscripcion(
            [FromBody]
            PreinscripcionDto preinscripcion,

            CancellationToken cancellationToken)
        {
            try
            {
                var nuevoId =
                    await _service
                        .CreatePreinscripcionAsync(
                            preinscripcion,
                            cancellationToken);

                return CreatedAtAction(
                    nameof(GetById),

                    new
                    {
                        id = nuevoId
                    },

                    new
                    {
                        alumnoId = nuevoId,

                        message =
                            "Preinscripción guardada correctamente."
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,

                    new
                    {
                        message =
                            "No se pudo guardar la preinscripción.",

                        detail =
                            ex.Message
                    });
            }
        }
    }
}

/*
//Lopez Melany
namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlumnoController : ControllerBase
    {
        private readonly IAlumnoService _service; // <- usar la interfaz

        public AlumnoController(IAlumnoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlumnoModelo>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var alumnosModelos = await _service.GetAlumnosModelos(cancellationToken);
                return Ok(alumnosModelos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
*/