using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces;

public interface IUsuarioService
{
    Task<Usuario?> AuthenticateAsync(string emailOrDni, string password, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default);
}
