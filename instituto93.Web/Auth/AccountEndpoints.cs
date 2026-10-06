using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using instituto93.Web.Services;

namespace instituto93.Web.Auth;

// La cookie solo se puede borrar desde una request HTTP, no desde el circuito de Blazor.
public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var account = endpoints.MapGroup("/account");

        // Los parámetros [FromForm] hacen que UseAntiforgery exija el token (400 si falta o es inválido).
        // Sin ellos el endpoint se ejecutaría sin validar nada.
        account.MapPost("/logout", async (
            HttpContext httpContext,
            AuthApiClient authApi,
            ILogger<AuthApiClient> logger,
            [FromForm] string? returnUrl) =>
        {
            var refreshToken = await httpContext.GetTokenAsync(AuthCookie.RefreshToken);
            if (!string.IsNullOrEmpty(refreshToken))
            {
                try
                {
                    await authApi.LogoutAsync(refreshToken, httpContext.RequestAborted);
                }
                catch (HttpRequestException ex)
                {
                    // La cookie se borra igual; el refresh token vencerá solo.
                    logger.LogWarning(ex, "No se pudo revocar el refresh token en la API.");
                }
            }

            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.LocalRedirect(LoginRedirect.LoginPath);
        });

        return endpoints;
    }
}
