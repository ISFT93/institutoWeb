using instituto93.Application.Interfaces;
using instituto93.Data.Repositories;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Controller.Seeds;

public sealed class DevelopmentUserSeed
{
    private const string Dni = "99999999";
    private const string DniDocente = "88888888";
    private const string Password = "PruebaInstituto93!";

    private readonly IAlumnoRepository _alumnoRepository;
    private readonly IProfesorRepository _profesorRepository;
    private readonly IAlumnoAccesoService _alumnoAccesoService;

    public DevelopmentUserSeed(
        IAlumnoRepository alumnoRepository,
        IProfesorRepository profesorRepository,
        IAlumnoAccesoService alumnoAccesoService)
    {
        _alumnoRepository = alumnoRepository;
        _profesorRepository = profesorRepository;
        _alumnoAccesoService = alumnoAccesoService;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedAlumnoAsync(cancellationToken);
        await SeedDocenteAsync(cancellationToken);
    }

    private async Task SeedAlumnoAsync(CancellationToken cancellationToken)
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

        await CrearContrasenaAsync(Dni, cancellationToken);
    }

    private async Task SeedDocenteAsync(CancellationToken cancellationToken)
    {
        var estado = await _alumnoAccesoService.ConsultarDniAsync(DniDocente, cancellationToken);
        if (estado == EstadoAccesoDni.ConContrasena)
            return;

        if (estado == EstadoAccesoDni.NoEncontrado)
        {
            await _profesorRepository.CreateAsync(new Profesores
            {
                Nombre = "Docente",
                Apellido = "Prueba",
                NumeroDocumento = DniDocente,
                FechaNacimiento = "1985-01-01",
                Sexo = 'M',
                Direccion = "Direccion de prueba",
                Localidad = "Localidad de prueba",
                Nacionalidad = "Argentina",
                Email = "docente.prueba@instituto93.local"
            }, cancellationToken);
        }

        await CrearContrasenaAsync(DniDocente, cancellationToken);
    }

    private async Task CrearContrasenaAsync(string dni, CancellationToken cancellationToken)
    {
        var resultado = await _alumnoAccesoService.CrearContrasenaAsync(dni, Password, Password, cancellationToken);
        if (resultado.Estado is not (CrearContrasenaEstado.Creada or CrearContrasenaEstado.YaTieneContrasena))
            throw new InvalidOperationException($"No se pudo crear el usuario de prueba {dni}: {resultado.Estado} {resultado.Mensaje}");
    }
}
