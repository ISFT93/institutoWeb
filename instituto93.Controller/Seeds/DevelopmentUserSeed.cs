using instituto93.Application.Interfaces;
using instituto93.Data.Repositories;
using instituto93.Domain.Models;

namespace instituto93.Controller.Seeds;

public sealed class DevelopmentUserSeed
{
    private const string Dni = "99999999";
    private const string Password = "PruebaInstituto93!";

    private readonly IAlumnoRepository _alumnoRepository;
    private readonly IAlumnoAccesoService _alumnoAccesoService;

    public DevelopmentUserSeed(
        IAlumnoRepository alumnoRepository,
        IAlumnoAccesoService alumnoAccesoService)
    {
        _alumnoRepository = alumnoRepository;
        _alumnoAccesoService = alumnoAccesoService;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var estado = await _alumnoAccesoService.ConsultarDniAsync(Dni, cancellationToken);
        if (estado == EstadoAccesoDni.ConContrasena)
            return;

        if (estado == EstadoAccesoDni.NoEncontrado)
        {
            await _alumnoRepository.CreateAsync(new AlumnoModelo
            {
                Nombre = "Usuario",
                Apellido = "Prueba",
                TipoDocumento = "Dni",
                NumeroDocumento = Dni,
                Sexo = "M",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Calle = "Direccion de prueba",
                Localidad = "Localidad de prueba",
                Email = "usuario.prueba@instituto93.local",
                Activo = true
            }, cancellationToken);
        }

        var resultado = await _alumnoAccesoService.CrearContrasenaAsync(Dni, Password, Password, cancellationToken);
        if (resultado.Estado is not (CrearContrasenaEstado.Creada or CrearContrasenaEstado.YaTieneContrasena))
            throw new InvalidOperationException($"No se pudo crear el usuario de prueba: {resultado.Estado} {resultado.Mensaje}");
    }
}
