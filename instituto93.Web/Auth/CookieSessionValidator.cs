using System.Security.Claims;
using instituto93.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace instituto93.Web.Auth;

// Corre en cada request HTTP autenticada (carga de página, conexión del circuito):
// la cookie solo es válida si su sesión del lado del servidor tiene tokens utilizables.
public static class CookieSessionValidator
{
    public static async Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        var sessionId = context.Principal?.FindFirstValue(AuthClaimTypes.SessionId);
        if (string.IsNullOrEmpty(sessionId))
        {
            await RejectAsync(context);
            return;
        }

        var sessions = context.HttpContext.RequestServices.GetRequiredService<TokenSessionManager>();

        TokenSet? tokens;
        try
        {
            tokens = await sessions.GetValidTokensAsync(sessionId, cancellationToken: context.HttpContext.RequestAborted);
        }
        catch (HttpRequestException)
        {
            // API caída: se conserva la sesión y se reintenta en la próxima request.
            return;
        }

        if (tokens is null)
        {
            await RejectAsync(context);
            return;
        }

        // Cada rotación extiende el refresh token; la cookie acompaña ese vencimiento.
        if (context.Properties.ExpiresUtc is { } cookieExpiresAt && tokens.RefreshTokenExpiresAt > cookieExpiresAt.AddMinutes(1))
        {
            context.Properties.IssuedUtc = DateTimeOffset.UtcNow;
            context.Properties.ExpiresUtc = tokens.RefreshTokenExpiresAt;
            context.ShouldRenew = true;
        }
    }

    private static async Task RejectAsync(CookieValidatePrincipalContext context)
    {
        context.RejectPrincipal();
        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
