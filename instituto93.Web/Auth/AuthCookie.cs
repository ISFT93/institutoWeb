using System.Globalization;
using System.Security.Claims;
using instituto93.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace instituto93.Web.Auth;

// Los tokens viven en la cookie de autenticación (AuthenticationProperties.StoreTokens),
// con los mismos nombres que usa el handler de OpenID Connect con SaveTokens = true.
public static class AuthCookie
{
    public const string AccessToken = "access_token";
    public const string RefreshToken = "refresh_token";
    public const string ExpiresAt = "expires_at";

    public static Task SignInAsync(HttpContext httpContext, TokenSet tokens, CurrentUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UsuarioId.ToString()),
            new(AuthClaimTypes.UsuarioId, user.UsuarioId.ToString()),
            new(ClaimTypes.Name, user.Nombre)
        };

        if (user.AlumnoId is int alumnoId)
            claims.Add(new Claim(AuthClaimTypes.AlumnoId, alumnoId.ToString()));

        if (user.ProfesorId is int profesorId)
            claims.Add(new Claim(AuthClaimTypes.ProfesorId, profesorId.ToString()));

        if (!string.IsNullOrWhiteSpace(user.Rol))
            claims.Add(new Claim(ClaimTypes.Role, user.Rol));

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

        var properties = new AuthenticationProperties
        {
            IsPersistent = true,
            AllowRefresh = true,
            IssuedUtc = DateTimeOffset.UtcNow,
            ExpiresUtc = tokens.RefreshTokenExpiresAt
        };
        StoreTokens(properties, tokens);

        return httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
    }

    public static void StoreTokens(AuthenticationProperties properties, TokenSet tokens) =>
        properties.StoreTokens(
        [
            new AuthenticationToken { Name = AccessToken, Value = tokens.AccessToken },
            new AuthenticationToken { Name = RefreshToken, Value = tokens.RefreshToken },
            new AuthenticationToken { Name = ExpiresAt, Value = tokens.AccessTokenExpiresAt.ToString("o", CultureInfo.InvariantCulture) }
        ]);
}
