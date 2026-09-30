namespace instituto93.Application.Interfaces;

public enum EstadoAccesoDni
{
    NoEncontrado,
    SinContrasena,
    ConContrasena
}

public enum CrearContrasenaEstado
{
    Creada,
    DniNoEncontrado,
    YaTieneContrasena,
    ContrasenaInvalida
}

public sealed record CrearContrasenaResultado(CrearContrasenaEstado Estado, string? Mensaje = null);

public interface IAlumnoAccesoService
{
    Task<EstadoAccesoDni> ConsultarDniAsync(string dni, CancellationToken cancellationToken = default);
    Task<CrearContrasenaResultado> CrearContrasenaAsync(
        string dni,
        string password,
        string confirmacion,
        CancellationToken cancellationToken = default);
}
