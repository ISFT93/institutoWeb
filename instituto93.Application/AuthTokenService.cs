using System.Security.Cryptography;
using System.Text;
using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace instituto93.Application;

public sealed class AuthTokenService : IAuthTokenService
{
    public const string AlumnoIdClaim = "alumnoId";

    private const int RefreshTokenBytes = 32;

    private static readonly JsonWebTokenHandler TokenHandler = new();

    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IUsuarioRepository _usuarios;
    private readonly JwtSettings _settings;
    private readonly ILogger<AuthTokenService> _logger;

    public AuthTokenService(
        IRefreshTokenRepository refreshTokens,
        IUsuarioRepository usuarios,
        JwtSettings settings,
        ILogger<AuthTokenService> logger)
    {
        _refreshTokens = refreshTokens ?? throw new ArgumentNullException(nameof(refreshTokens));
        _usuarios = usuarios ?? throw new ArgumentNullException(nameof(usuarios));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TokenPair> IssueAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        await _refreshTokens.DeleteInactiveFamiliesAsync(usuario.Id, cancellationToken);

        var now = UtcNowSeconds();
        var (refreshToken, entity) = CreateRefreshToken(
            usuario.Id,
            Guid.NewGuid(),
            now,
            now.Add(_settings.RefreshTokenAbsoluteLifetime));

        await _refreshTokens.AddAsync(entity, cancellationToken);

        return CreatePair(usuario, now, refreshToken, entity.ExpiresAt);
    }

    public async Task<TokenPair?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var current = await _refreshTokens.GetByHashAsync(Hash(refreshToken), cancellationToken);
        if (current is null || current.RevokedAt is not null)
            return null;

        if (current.UsedAt is not null)
        {
            await RevokeFamilyAfterReuseAsync(current, cancellationToken);
            return null;
        }

        var now = UtcNowSeconds();
        if (current.ExpiresAt <= now)
            return null;

        var usuario = await _usuarios.GetByIdAsync(current.UsuarioId, cancellationToken);
        if (usuario is null || !usuario.Activo || usuario.Alumno?.Activo == false)
        {
            await _refreshTokens.RevokeFamilyAsync(current.FamilyId, cancellationToken);
            return null;
        }

        var (nextToken, next) = CreateRefreshToken(usuario.Id, current.FamilyId, now, current.FamilyExpiresAt);

        if (!await _refreshTokens.RotateAsync(current.Id, next, cancellationToken))
        {
            // Otro request usó este mismo token entre la lectura y la rotación: se trata como reutilización.
            await RevokeFamilyAfterReuseAsync(current, cancellationToken);
            return null;
        }

        return CreatePair(usuario, now, nextToken, next.ExpiresAt);
    }

    public async Task RevokeAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return;

        var current = await _refreshTokens.GetByHashAsync(Hash(refreshToken), cancellationToken);
        if (current is not null)
            await _refreshTokens.RevokeFamilyAsync(current.FamilyId, cancellationToken);
    }

    private async Task RevokeFamilyAfterReuseAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Reutilización de refresh token detectada (usuario {UsuarioId}, familia {FamilyId}). Se revoca la sesión.",
            token.UsuarioId,
            token.FamilyId);

        await _refreshTokens.RevokeFamilyAsync(token.FamilyId, cancellationToken);
    }

    private TokenPair CreatePair(Usuario usuario, DateTime now, string refreshToken, DateTime refreshTokenExpiresAt)
    {
        var accessTokenExpiresAt = now.Add(_settings.AccessTokenLifetime);
        var alumno = usuario.Alumno;

        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Sub] = usuario.Id.ToString(),
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString("N"),
            [AlumnoIdClaim] = usuario.AlumnoId.ToString()
        };

        if (alumno is not null)
            claims[JwtRegisteredClaimNames.Name] = $"{alumno.Nombre} {alumno.Apellido}".Trim();

        if (!string.IsNullOrWhiteSpace(alumno?.Email))
            claims[JwtRegisteredClaimNames.Email] = alumno.Email;

        var accessToken = TokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            Claims = claims,
            IssuedAt = now,
            NotBefore = now,
            Expires = accessTokenExpiresAt,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_settings.SigningKey),
                SecurityAlgorithms.HmacSha256)
        });

        return new TokenPair(accessToken, accessTokenExpiresAt, refreshToken, refreshTokenExpiresAt);
    }

    private (string Token, RefreshToken Entity) CreateRefreshToken(
        int usuarioId,
        Guid familyId,
        DateTime now,
        DateTime familyExpiresAt)
    {
        var token = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(RefreshTokenBytes));
        var slidingExpiresAt = now.Add(_settings.RefreshTokenLifetime);

        return (token, new RefreshToken
        {
            UsuarioId = usuarioId,
            FamilyId = familyId,
            TokenHash = Hash(token),
            CreatedAt = now,
            ExpiresAt = slidingExpiresAt < familyExpiresAt ? slidingExpiresAt : familyExpiresAt,
            FamilyExpiresAt = familyExpiresAt
        });
    }

    private static byte[] Hash(string token) => SHA256.HashData(Encoding.UTF8.GetBytes(token));

    // La base guarda DATETIME2(0); truncar evita diferencias entre lo emitido y lo persistido.
    private static DateTime UtcNowSeconds()
    {
        var now = DateTime.UtcNow;
        return new DateTime(now.Ticks - now.Ticks % TimeSpan.TicksPerSecond, DateTimeKind.Utc);
    }
}
