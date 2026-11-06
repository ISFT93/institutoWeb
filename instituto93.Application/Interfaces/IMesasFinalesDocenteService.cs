using instituto93.Domain.DTOs;

namespace instituto93.Application.Interfaces;

public interface IMesasFinalesDocenteService
{
    Task<List<MesaFinalDocenteDto>> GetByProfesorAsync(int profesorId, CancellationToken cancellationToken = default);
    Task<MesaFinalDetalleDto?> GetByIdAsync(int mesaId, int profesorId, CancellationToken cancellationToken = default);
    Task<bool> GuardarNotasAsync(
        int mesaId,
        int profesorId,
        IReadOnlyCollection<GuardarNotaFinalDto> notas,
        CancellationToken cancellationToken = default);
}
