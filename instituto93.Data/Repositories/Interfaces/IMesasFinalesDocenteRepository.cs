using instituto93.Domain.DTOs;

namespace instituto93.Data.Repositories.Interfaces;

public interface IMesasFinalesDocenteRepository
{
    Task<List<MesaFinalDocenteDto>> GetByProfesorAsync(
        int profesorId,
        CancellationToken cancellationToken = default);

    Task<MesaFinalDetalleDto?> GetByIdAsync(
        int mesaId,
        int profesorId,
        CancellationToken cancellationToken = default);

    Task<bool> GuardarNotasAsync(
        int mesaId,
        int profesorId,
        IReadOnlyCollection<GuardarNotaFinalDto> notas,
        CancellationToken cancellationToken = default);
}
