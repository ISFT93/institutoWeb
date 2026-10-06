namespace instituto93.Application;

public sealed class JwtSettings
{
    public const int MinimumSecretBytes = 32;

    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required byte[] SigningKey { get; init; }
    public TimeSpan AccessTokenLifetime { get; init; } = TimeSpan.FromMinutes(15);

    // Vigencia deslizante: cada rotación extiende el refresh token hasta este plazo...
    public TimeSpan RefreshTokenLifetime { get; init; } = TimeSpan.FromDays(14);

    // ...sin superar nunca este máximo contado desde el login.
    public TimeSpan RefreshTokenAbsoluteLifetime { get; init; } = TimeSpan.FromDays(30);
}
