using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.DTOs;
using instituto93.Domain.Rules;

namespace instituto93.Application;

public sealed class MateriasDocenteService(IMateriasDocenteRepository repository) : IMateriasDocenteService
{
    public Task<List<MateriaDocenteDto>> GetByProfesorAsync(int profesorId, CancellationToken cancellationToken = default)
    {
        Validar(profesorId, nameof(profesorId));
        return repository.GetByProfesorAsync(profesorId, cancellationToken);
    }

    public Task<MateriaDocenteDetalleDto?> GetByIdAsync(int cursadaId, int profesorId, CancellationToken cancellationToken = default)
    {
        Validar(cursadaId, nameof(cursadaId));
        Validar(profesorId, nameof(profesorId));
        return repository.GetByIdAsync(cursadaId, profesorId, cancellationToken);
    }

    public Task<List<TipoEvaluacionDto>> GetTiposAsync(CancellationToken ct = default) => repository.GetTiposAsync(ct);

    public Task<List<EvaluacionDocenteDto>?> GetEvaluacionesAsync(int cursadaId, int profesorId, CancellationToken ct = default)
    {
        Validar(cursadaId, nameof(cursadaId));
        Validar(profesorId, nameof(profesorId));
        return repository.GetEvaluacionesAsync(cursadaId, profesorId, ct);
    }

    public Task<int?> CrearEvaluacionAsync(int cursadaId, int profesorId, CrearEvaluacionDto r, CancellationToken ct = default)
    {
        Validar(cursadaId, nameof(cursadaId));
        Validar(profesorId, nameof(profesorId));
        ValidarTipo(r.TipoEvaluacionId);
        if (string.IsNullOrWhiteSpace(r.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");
        if (r.Fecha.Date < PrimerDiaMesActual())
            throw new ArgumentException("La fecha debe pertenecer al mes actual o a uno posterior.");
        if (r.Cuatrimestre is not (1 or 2))
            throw new ArgumentException("El cuatrimestre no es válido.");
        return repository.CrearEvaluacionAsync(cursadaId, profesorId, r, ct);
    }

    public Task<bool> ActualizarEvaluacionAsync(int id, int profesorId, ActualizarEvaluacionDto r, CancellationToken ct = default)
    {
        Validar(id, nameof(id));
        Validar(profesorId, nameof(profesorId));
        ValidarTipo(r.TipoEvaluacionId);
        if (string.IsNullOrWhiteSpace(r.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");
        if (r.Cuatrimestre is not (1 or 2))
            throw new ArgumentException("El cuatrimestre no es válido.");
        if (r.Estado is not ("Borrador" or "Activa" or "Cerrada"))
            throw new ArgumentException("El estado no es válido.");
        if (r.Ponderacion is < 0 or > 100)
            throw new ArgumentException("La ponderación debe estar entre 0 y 100.");
        if (string.IsNullOrWhiteSpace(r.Version))
            throw new ArgumentException("La versión es obligatoria.");
        return repository.ActualizarEvaluacionAsync(id, profesorId, r, ct);
    }

    public Task<EvaluacionNotasDto?> GetNotasAsync(int id, int profesorId, CancellationToken ct = default)
    {
        Validar(id, nameof(id));
        Validar(profesorId, nameof(profesorId));
        return repository.GetNotasAsync(id, profesorId, ct);
    }

    public Task<bool> GuardarNotasAsync(int id, int profesorId, IReadOnlyCollection<GuardarNotaAlumnoDto> notas, CancellationToken ct = default)
    {
        Validar(id, nameof(id));
        Validar(profesorId, nameof(profesorId));
        if (notas.Any(n => !NotaEvaluacionRules.EsValida(n.Nota)))
            throw new ArgumentException("Las notas deben estar entre 0 y 10.");
        if (notas.Any(n => n.Ausente && n.Nota is not null))
            throw new ArgumentException("Un alumno ausente no puede tener nota.");
        return repository.GuardarNotasAsync(id, profesorId, notas, ct);
    }

    private static void Validar(int id, string nombre)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nombre);
    }

    private static void ValidarTipo(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El tipo de evaluación no es válido.");
    }

    private static DateTime PrimerDiaMesActual() => new(DateTime.Today.Year, DateTime.Today.Month, 1);
}
