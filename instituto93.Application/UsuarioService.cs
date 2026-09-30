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
        string dni,
        string password,
        CancellationToken cancellationToken = default)
    {
        dni = AlumnoAccesoService.NormalizarDni(dni);
        if (dni.Length == 0)
            return null;

        var usuario = await _repository.GetByDniAsync(dni, cancellationToken);
        return usuario is not null && usuario.Activo && _repository.VerifyPassword(usuario.Password, password)
            ? usuario
            : null;
    }
}
