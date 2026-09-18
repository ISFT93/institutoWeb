using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByEmailOrDniAsync(string emailOrDni, CancellationToken cancellationToken = default);
    Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default);
    bool VerifyPassword(string storedHash, string password);
}
