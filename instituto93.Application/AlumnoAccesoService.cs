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
    private readonly IUsuarioRepository _usuarios;

    public AlumnoAccesoService(IAlumnoRepository alumnos, IUsuarioRepository usuarios)
    {
        _alumnos = alumnos ?? throw new ArgumentNullException(nameof(alumnos));
        _usuarios = usuarios ?? throw new ArgumentNullException(nameof(usuarios));
    }

    public static string NormalizarDni(string? dni)
    {
        return string.Concat((dni ?? string.Empty).Where(c => !char.IsWhiteSpace(c) && c != '.' && c != '-'));
    }

    public async Task<EstadoAccesoDni> ConsultarDniAsync(string dni, CancellationToken cancellationToken = default)
    {
        var alumno = await BuscarAlumnoAsync(dni, cancellationToken);
        if (alumno is null)
            return EstadoAccesoDni.NoEncontrado;

        return await _usuarios.GetByAlumnoIdAsync(alumno.AlumnoId, cancellationToken) is null
            ? EstadoAccesoDni.SinContrasena
            : EstadoAccesoDni.ConContrasena;
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

        var alumno = await BuscarAlumnoAsync(dni, cancellationToken);
        if (alumno is null)
            return new CrearContrasenaResultado(CrearContrasenaEstado.DniNoEncontrado);

        if (await _usuarios.GetByAlumnoIdAsync(alumno.AlumnoId, cancellationToken) is not null)
            return new CrearContrasenaResultado(CrearContrasenaEstado.YaTieneContrasena);

        try
        {
            await _usuarios.AddAsync(new Usuario
            {
                AlumnoId = alumno.AlumnoId,
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

    private async Task<AlumnoModelo?> BuscarAlumnoAsync(string dni, CancellationToken cancellationToken)
    {
        dni = NormalizarDni(dni);
        return dni.Length == 0 ? null : await _alumnos.GetByDocumentoAsync(dni, cancellationToken);
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
