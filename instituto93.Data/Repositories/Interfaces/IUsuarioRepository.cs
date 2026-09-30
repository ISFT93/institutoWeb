using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByAlumnoIdAsync(int alumnoId, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByDniAsync(string dni, CancellationToken cancellationToken = default);
    Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default);
    bool VerifyPassword(string storedHash, string password);
}
