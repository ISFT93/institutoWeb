using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using instituto93.Web.Services;

namespace instituto93.Web.Auth;

// Renueva el access token cuando está por vencer y reescribe la cookie con los tokens nuevos.
// Solo puede correr en una request HTTP (carga de página o conexión del circuito): una vez
// abierto el circuito, los tokens quedan fijos hasta la próxima request (dotnet/aspnetcore#55213).
public static class CookieTokenRefresher
{
    private static readonly TimeSpan RefreshMargin = TimeSpan.FromMinutes(5);

    public static async Task ValidateOrRefreshAsync(CookieValidatePrincipalContext context)
    {
        var refreshToken = context.Properties.GetTokenValue(AuthCookie.RefreshToken);
        if (string.IsNullOrEmpty(refreshToken) ||
            !DateTimeOffset.TryParse(
                context.Properties.GetTokenValue(AuthCookie.ExpiresAt),
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind,
                out var accessTokenExpiresAt))
        {
            await RejectAsync(context);
            return;
        }

        if (DateTimeOffset.UtcNow + RefreshMargin < accessTokenExpiresAt)
            return;

        var authApi = context.HttpContext.RequestServices.GetRequiredService<AuthApiClient>();

        try
        {
            // Sin cancelación: si la API ya rotó el refresh token y se corta la request antes de
            // reescribir la cookie, el refresh token viejo quedaría inservible.
            var tokens = await authApi.RefreshAsync(refreshToken, CancellationToken.None);
            if (tokens is null)
            {
                await RejectAsync(context);
                return;
            }

            AuthCookie.StoreTokens(context.Properties, tokens);
            context.ShouldRenew = true;
        }
        catch (HttpRequestException)
        {
            // API caída: se conserva la sesión y se reintenta en la próxima request.
        }
    }

    private static async Task RejectAsync(CookieValidatePrincipalContext context)
    {
        context.RejectPrincipal();
        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
