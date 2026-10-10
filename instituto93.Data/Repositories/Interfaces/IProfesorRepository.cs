using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces;

public interface IProfesorRepository
{
    Task<Profesores?> GetByDocumentoAsync(string numeroDocumento, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Profesores profesor, CancellationToken cancellationToken = default);
}
