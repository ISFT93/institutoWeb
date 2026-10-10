using instituto93.Application.Interfaces;
using instituto93.Data.Repositories;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using Microsoft.Data.SqlClient;

namespace instituto93.Application;

public sealed class AlumnoAccesoService : IAlumnoAccesoService
{
    public const int LongitudMinimaContrasena = 8;

    private const int SqlUniqueConstraintViolation = 2627;
    private const int SqlUniqueIndexViolation = 2601;

    private readonly IAlumnoRepository _alumnos;
    private readonly IProfesorRepository _profesores;
    private readonly IUsuarioRepository _usuarios;

    private sealed record PersonaAcceso(int? AlumnoId, int? ProfesorId, string? Nombre);

    public AlumnoAccesoService(
        IAlumnoRepository alumnos,
        IProfesorRepository profesores,
        IUsuarioRepository usuarios)
    {
        _alumnos = alumnos ?? throw new ArgumentNullException(nameof(alumnos));
        _profesores = profesores ?? throw new ArgumentNullException(nameof(profesores));
        _usuarios = usuarios ?? throw new ArgumentNullException(nameof(usuarios));
    }

    public static string NormalizarDni(string? dni)
    {
        return string.Concat((dni ?? string.Empty).Where(c => !char.IsWhiteSpace(c) && c != '.' && c != '-'));
    }

    public async Task<EstadoAccesoDni> ConsultarDniAsync(string dni, CancellationToken cancellationToken = default)
    {
        return (await ConsultarAccesoAsync(dni, cancellationToken)).Estado;
    }

    public async Task<(EstadoAccesoDni Estado, string? Nombre)> ConsultarAccesoAsync(string dni, CancellationToken cancellationToken = default)
    {
        var persona = await BuscarPersonaAsync(dni, cancellationToken);
        if (persona is null)
            return (EstadoAccesoDni.NoEncontrado, null);

        var estado = await BuscarUsuarioAsync(persona, cancellationToken) is null
            ? EstadoAccesoDni.SinContrasena
            : EstadoAccesoDni.ConContrasena;

        return (estado, persona.Nombre);
    }

    public async Task<CrearContrasenaResultado> CrearContrasenaAsync(
        string dni,
        string password,
        string confirmacion,
        CancellationToken cancellationToken = default)
    {
        var error = ValidarContrasena(password, confirmacion);
        if (error is not null)
            return new CrearContrasenaResultado(CrearContrasenaEstado.ContrasenaInvalida, error);

        var persona = await BuscarPersonaAsync(dni, cancellationToken);
        if (persona is null)
            return new CrearContrasenaResultado(CrearContrasenaEstado.DniNoEncontrado);

        if (await BuscarUsuarioAsync(persona, cancellationToken) is not null)
            return new CrearContrasenaResultado(CrearContrasenaEstado.YaTieneContrasena);

        try
        {
            await _usuarios.AddAsync(new Usuario
            {
                AlumnoId = persona.AlumnoId,
                ProfesorId = persona.ProfesorId,
                Password = password,
                Activo = true
            }, cancellationToken);
        }
        catch (SqlException ex) when (ex.Number is SqlUniqueConstraintViolation or SqlUniqueIndexViolation)
        {
            return new CrearContrasenaResultado(CrearContrasenaEstado.YaTieneContrasena);
        }

        return new CrearContrasenaResultado(CrearContrasenaEstado.Creada);
    }

    // Si el DNI existe como docente y como alumno, gana el docente.
    private async Task<PersonaAcceso?> BuscarPersonaAsync(string dni, CancellationToken cancellationToken)
    {
        dni = NormalizarDni(dni);
        if (dni.Length == 0)
            return null;

        var profesor = await _profesores.GetByDocumentoAsync(dni, cancellationToken);
        if (profesor is not null)
            return new PersonaAcceso(null, profesor.ProfesorId, profesor.Nombre?.Trim());

        var alumno = await _alumnos.GetByDocumentoAsync(dni, cancellationToken);
        return alumno is null ? null : new PersonaAcceso(alumno.AlumnoId, null, alumno.Nombre?.Trim());
    }

    private Task<Usuario?> BuscarUsuarioAsync(PersonaAcceso persona, CancellationToken cancellationToken)
    {
        return persona.ProfesorId is int profesorId
            ? _usuarios.GetByProfesorIdAsync(profesorId, cancellationToken)
            : _usuarios.GetByAlumnoIdAsync(persona.AlumnoId!.Value, cancellationToken);
    }

    private static string? ValidarContrasena(string? password, string? confirmacion)
    {
        if (string.IsNullOrWhiteSpace(password))
            return "La contraseña es obligatoria.";

        if (password.Length < LongitudMinimaContrasena)
            return $"La contraseña debe tener al menos {LongitudMinimaContrasena} caracteres.";

        if (password != confirmacion)
            return "Las contraseñas no coinciden.";

        return null;
    }
}
