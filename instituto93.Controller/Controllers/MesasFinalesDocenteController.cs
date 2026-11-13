using System.Security.Claims;
using System.Data;
using instituto93.Application;
using instituto93.Application.Interfaces;
using instituto93.Domain.DTOs;
using instituto93.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace instituto93.Controller.Controllers;

[ApiController]
[Route("api/mesas-finales/docente")]
[Authorize(Roles = Rol.Docente)]
public sealed class MesasFinalesDocenteController(
    IMesasFinalesDocenteService service,
    ILogger<MesasFinalesDocenteController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMesas(CancellationToken cancellationToken)
    {
        if (!TryGetProfesorId(out var profesorId))
            return Unauthorized();

        return Ok(await service.GetByProfesorAsync(profesorId, cancellationToken));
    }

    [HttpGet("{mesaId:int}")]
    public async Task<IActionResult> GetMesa(int mesaId, CancellationToken cancellationToken)
    {
        if (!TryGetProfesorId(out var profesorId))
            return Unauthorized();

        var mesa = await service.GetByIdAsync(mesaId, profesorId, cancellationToken);
        return mesa is null ? NotFound() : Ok(mesa);
    }

    [HttpPut("{mesaId:int}/notas")]
    public async Task<IActionResult> GuardarNotas(
        int mesaId,
        [FromBody] GuardarNotasFinalesDto request,
        CancellationToken cancellationToken)
    {
        if (!TryGetProfesorId(out var profesorId))
            return Unauthorized();

        if (request.Notas is null)
            return BadRequest(new { message = "La lista de notas es obligatoria." });

        try
        {
            var guardado = await service.GuardarNotasAsync(
                mesaId,
                profesorId,
                request.Notas,
                request.CerrarMesa,
                cancellationToken);

            return guardado ? NoContent() : NotFound();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (DBConcurrencyException)
        {
            return Conflict(new { message = "Las notas fueron modificadas por otra persona. Recargá la mesa antes de volver a guardar." });
        }
        catch (SqlException exception)
        {
            logger.LogError(exception, "Error al guardar las notas de la mesa {MesaId}. Cierre solicitado: {CerrarMesa}",
                mesaId, request.CerrarMesa);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = request.CerrarMesa
                    ? "No se pudo confirmar el cierre de la mesa ni el guardado del analítico. Recargá la mesa y verificá su estado."
                    : "No se pudieron guardar las notas. Recargá la mesa y verificá su estado."
            });
        }
    }

    private bool TryGetProfesorId(out int profesorId) =>
        int.TryParse(User.FindFirstValue(AuthTokenService.ProfesorIdClaim), out profesorId);
}
