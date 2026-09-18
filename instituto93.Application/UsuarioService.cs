using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application;

public sealed class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Usuario?> AuthenticateAsync(
        string emailOrDni,
        string password,
        CancellationToken cancellationToken = default)
    {
        var usuario = await _repository.GetByEmailOrDniAsync(emailOrDni, cancellationToken);
        return usuario is not null && usuario.activo && _repository.VerifyPassword(usuario.Password, password)
            ? usuario
            : null;
    }

    public Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _repository.GetByEmailAsync(email, cancellationToken);
    }

    public Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        return _repository.AddAsync(usuario, cancellationToken);
    }
}
