using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces;

public interface IUsuarioService
{
    Task<Usuario?> AuthenticateAsync(string dni, string password, CancellationToken cancellationToken = default);
}
