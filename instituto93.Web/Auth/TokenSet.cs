namespace instituto93.Web.Auth;

public sealed record TokenSet(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt);

public sealed record CurrentUser(int UsuarioId, int AlumnoId, string Nombre, string? Email);

public static class AuthClaimTypes
{
    // Identificador de la sesión del lado del servidor donde viven los tokens.
    public const string SessionId = "sid";
    public const string UsuarioId = "usuarioId";
    public const string AlumnoId = "alumnoId";
}
