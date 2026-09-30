using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace instituto93.Web.Auth;

// Endpoints HTTP (fuera del circuito de Blazor) que pueden escribir y borrar la cookie.
// Ambos exigen el token antiforgery. Se valida explícitamente porque UseAntiforgery solo
// revisa requests con cuerpo de formulario.
public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var account = endpoints.MapGroup("/account")
            .AddEndpointFilter(async (context, next) =>
            {
                var antiforgery = context.HttpContext.RequestServices.GetRequiredService<IAntiforgery>();
                return await antiforgery.IsRequestValidAsync(context.HttpContext)
                    ? await next(context)
                    : Results.BadRequest();
            });

        account.MapPost("/complete-login", async (
            HttpContext httpContext,
            [FromForm] string? ticket,
            [FromForm] string? returnUrl,
            LoginTicketStore loginTickets,
            TokenSessionManager sessions) =>
        {
            var login = string.IsNullOrEmpty(ticket)
                ? null
                : await loginTickets.RedeemAsync(ticket, httpContext.RequestAborted);

            if (login is null)
                return Results.LocalRedirect(LoginRedirect.LoginPath);

            var previousSessionId = httpContext.User.FindFirstValue(AuthClaimTypes.SessionId);
            if (!string.IsNullOrEmpty(previousSessionId))
                await sessions.EndAsync(previousSessionId, httpContext.RequestAborted);

            var sessionId = await sessions.StartAsync(login.Tokens, httpContext.RequestAborted);
            var user = login.User;

            var claims = new List<Claim>
            {
                new(AuthClaimTypes.SessionId, sessionId),
                new(ClaimTypes.NameIdentifier, user.UsuarioId.ToString()),
                new(AuthClaimTypes.UsuarioId, user.UsuarioId.ToString()),
                new(AuthClaimTypes.AlumnoId, user.AlumnoId.ToString()),
                new(ClaimTypes.Name, user.Nombre)
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = login.Tokens.RefreshTokenExpiresAt
                });

            return Results.LocalRedirect(LoginRedirect.SanitizeReturnUrl(returnUrl));
        });

        account.MapPost("/logout", async (
            HttpContext httpContext,
            TokenSessionManager sessions) =>
        {
            var sessionId = httpContext.User.FindFirstValue(AuthClaimTypes.SessionId);
            if (!string.IsNullOrEmpty(sessionId))
                await sessions.EndAsync(sessionId, httpContext.RequestAborted);

            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.LocalRedirect(LoginRedirect.LoginPath);
        });

        return endpoints;
    }
}
