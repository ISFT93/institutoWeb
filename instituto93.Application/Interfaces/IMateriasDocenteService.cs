using instituto93.Domain.DTOs;
namespace instituto93.Application.Interfaces;
public interface IMateriasDocenteService
{
    Task<List<MateriaDocenteDto>> GetByProfesorAsync(int profesorId, CancellationToken cancellationToken=default);
    Task<MateriaDocenteDetalleDto?> GetByIdAsync(int cursadaId,int profesorId,CancellationToken cancellationToken=default);
    Task<List<TipoEvaluacionDto>> GetTiposAsync(CancellationToken cancellationToken=default);
    Task<List<EvaluacionDocenteDto>?> GetEvaluacionesAsync(int cursadaId,int profesorId,CancellationToken cancellationToken=default);
    Task<int?> CrearEvaluacionAsync(int cursadaId,int profesorId,CrearEvaluacionDto request,CancellationToken cancellationToken=default);
    Task<bool> ActualizarEvaluacionAsync(int evaluacionId,int profesorId,ActualizarEvaluacionDto request,CancellationToken cancellationToken=default);
    Task<EvaluacionNotasDto?> GetNotasAsync(int evaluacionId,int profesorId,CancellationToken cancellationToken=default);
    Task<bool> GuardarNotasAsync(int evaluacionId,int profesorId,IReadOnlyCollection<GuardarNotaAlumnoDto> notas,CancellationToken cancellationToken=default);
}
