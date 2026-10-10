namespace instituto93.Web.Models;

public sealed record CurrentUser(int UsuarioId, int? AlumnoId, int? ProfesorId, string Rol, string Nombre, string? Email);
