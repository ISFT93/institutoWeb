using System.Security.Claims;
using System.Data;
using instituto93.Application;
using instituto93.Application.Interfaces;
using instituto93.Domain.Models;
using instituto93.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace instituto93.Controller.Controllers;
[ApiController]
[Route("api/materias/docente")]
[Authorize(Roles=Rol.Docente)]
public sealed class MateriasDocenteController(IMateriasDocenteService service):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct) => TryProfesor(out var id) ? Ok(await service.GetByProfesorAsync(id,ct)) : Unauthorized();
    [HttpGet("{cursadaId:int}")]
    public async Task<IActionResult> Get(int cursadaId,CancellationToken ct)
    {
        if(!TryProfesor(out var id)) return Unauthorized();
        var result=await service.GetByIdAsync(cursadaId,id,ct);
        return result is null?NotFound():Ok(result);
    }
    private bool TryProfesor(out int id)=>int.TryParse(User.FindFirstValue(AuthTokenService.ProfesorIdClaim),out id);
    [HttpGet("tipos-evaluacion")]
    public async Task<IActionResult> Tipos(CancellationToken ct)=>Ok(await service.GetTiposAsync(ct));
    [HttpGet("{cursadaId:int}/evaluaciones")]
    public async Task<IActionResult> Evaluaciones(int cursadaId,CancellationToken ct){if(!TryProfesor(out var id))return Unauthorized();var r=await service.GetEvaluacionesAsync(cursadaId,id,ct);return r is null?NotFound():Ok(r);}
    [HttpPost("{cursadaId:int}/evaluaciones")]
    public async Task<IActionResult> Crear(int cursadaId,[FromBody] CrearEvaluacionDto request,CancellationToken ct){if(!TryProfesor(out var id))return Unauthorized();try{var nuevo=await service.CrearEvaluacionAsync(cursadaId,id,request,ct);return nuevo is null?NotFound():Created($"api/materias/docente/evaluaciones/{nuevo}",new{evaluacionId=nuevo});}catch(ArgumentException e){return BadRequest(new{message=e.Message});}}
    [HttpGet("evaluaciones/{evaluacionId:int}")]
    public async Task<IActionResult> Notas(int evaluacionId,CancellationToken ct){if(!TryProfesor(out var id))return Unauthorized();var r=await service.GetNotasAsync(evaluacionId,id,ct);return r is null?NotFound():Ok(r);}
    [HttpPut("evaluaciones/{evaluacionId:int}")]
    public async Task<IActionResult> Actualizar(int evaluacionId,[FromBody] ActualizarEvaluacionDto request,CancellationToken ct){if(!TryProfesor(out var id))return Unauthorized();try{return await service.ActualizarEvaluacionAsync(evaluacionId,id,request,ct)?NoContent():Conflict(new{message="La evaluación fue modificada. Recargá la página."});}catch(ArgumentException e){return BadRequest(new{message=e.Message});}}
    [HttpPut("evaluaciones/{evaluacionId:int}/notas")]
    public async Task<IActionResult> Guardar(int evaluacionId,[FromBody] GuardarNotasEvaluacionDto request,CancellationToken ct){if(!TryProfesor(out var id))return Unauthorized();try{return await service.GuardarNotasAsync(evaluacionId,id,request.Notas??[],ct)?NoContent():Conflict(new{message="La evaluación debe estar activa para cargar notas."});}catch(ArgumentException e){return BadRequest(new{message=e.Message});}catch(DBConcurrencyException){return Conflict(new{message="Las notas fueron modificadas por otra persona. Recargá la evaluación."});}}
}
