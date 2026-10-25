using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.DTOs;
using instituto93.Domain.Rules;

namespace instituto93.Application;

public sealed class MesasFinalesDocenteService(IMesasFinalesDocenteRepository repository)
    : IMesasFinalesDocenteService
{
    public Task<List<MesaFinalDocenteDto>> GetByProfesorAsync(
        int profesorId,
        CancellationToken cancellationToken = default)
    {
        ValidarProfesorId(profesorId);
        return repository.GetByProfesorAsync(profesorId, cancellationToken);
    }

    public Task<MesaFinalDetalleDto?> GetByIdAsync(
        int mesaId,
        int profesorId,
        CancellationToken cancellationToken = default)
    {
        ValidarMesaId(mesaId);
        ValidarProfesorId(profesorId);
        return repository.GetByIdAsync(mesaId, profesorId, cancellationToken);
    }

    public Task<bool> GuardarNotasAsync(
        int mesaId,
        int profesorId,
        IReadOnlyCollection<GuardarNotaFinalDto> notas,
        CancellationToken cancellationToken = default)
    {
        ValidarMesaId(mesaId);
        ValidarProfesorId(profesorId);
        ArgumentNullException.ThrowIfNull(notas);

        if (notas.Count == 0)
            throw new ArgumentException("No se enviaron notas para guardar.", nameof(notas));

        if (notas.Any(nota => nota.MesaAlumnoId <= 0))
            throw new ArgumentException("Una de las inscripciones no es válida.", nameof(notas));

        if (notas.GroupBy(nota => nota.MesaAlumnoId).Any(grupo => grupo.Count() > 1))
            throw new ArgumentException("No se puede enviar más de una nota para la misma inscripción.", nameof(notas));

        if (notas.Any(nota => nota.Ausente && nota.Nota is not null))
            throw new ArgumentException("Un alumno ausente no puede tener una nota.", nameof(notas));

        if (notas.Any(nota => !nota.Ausente && !NotaFinalRules.EsValida(nota.Nota)))
            throw new ArgumentException(
                $"Las notas deben estar entre {NotaFinalRules.Minima:0.##} y {NotaFinalRules.Maxima:0.##}.",
                nameof(notas));

        return repository.GuardarNotasAsync(mesaId, profesorId, notas, cancellationToken);
    }

    private static void ValidarMesaId(int mesaId)
    {
        if (mesaId <= 0)
            throw new ArgumentOutOfRangeException(nameof(mesaId));
    }

    private static void ValidarProfesorId(int profesorId)
    {
        if (profesorId <= 0)
            throw new ArgumentOutOfRangeException(nameof(profesorId));
    }
}
