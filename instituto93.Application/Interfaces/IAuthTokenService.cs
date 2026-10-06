using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces;

public sealed record TokenPair(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);

public interface IAuthTokenService
{
    // Emite un access token y un refresh token de una nueva familia (nuevo login).
    Task<TokenPair> IssueAsync(Usuario usuario, CancellationToken cancellationToken = default);

    // Rota el refresh token. Devuelve null si es inválido, expiró, fue revocado o se reutilizó.
    Task<TokenPair?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);

    // Revoca la familia del refresh token (logout). No falla si el token no existe.
    Task RevokeAsync(string refreshToken, CancellationToken cancellationToken = default);
}
