using instituto93.Application.Interfaces;
using instituto93.Data.Repositories;
using instituto93.Domain.Models;

namespace instituto93.Controller.Seeds;

public sealed class DevelopmentUserSeed
{
    private const string Email = "usuario.prueba@instituto93.local";
    private const string Password = "PruebaInstituto93!";
    private const string LocalidadNombre = "Localidad de prueba";

    private readonly IUsuarioService _usuarioService;
    private readonly ILocalidadRepository _localidadRepository;

    public DevelopmentUserSeed(
        IUsuarioService usuarioService,
        ILocalidadRepository localidadRepository)
    {
        _usuarioService = usuarioService;
        _localidadRepository = localidadRepository;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _usuarioService.GetByEmailAsync(Email, cancellationToken) is not null)
            return;

        var localidad = (await _localidadRepository.GetAllAsync(cancellationToken))
            .FirstOrDefault(localidad => localidad.Nombre == LocalidadNombre);

        var localidadId = localidad?.Id
            ?? await _localidadRepository.CreateAsync(
                new Localidad { Nombre = LocalidadNombre },
                cancellationToken);

        await _usuarioService.AddAsync(new Usuario
        {
            Nombre = "Usuario",
            Apellido = "Prueba",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Email = Email,
            Password = Password,
            Dni = "99999999",
            Telefono = "1100000000",
            Direccion = "Direccion de prueba",
            LocalidadId = localidadId,
            activo = true
        }, cancellationToken);
    }
}
