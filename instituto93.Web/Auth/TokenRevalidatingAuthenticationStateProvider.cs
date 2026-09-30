using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace instituto93.Web.Auth;

// Un circuito puede durar horas sin requests HTTP. Revalida periódicamente la sesión
// (renovando los tokens si hace falta) y, si ya no es válida, deja al usuario como anónimo
// para que AuthorizeRouteView lo redirija a /login.
public sealed class TokenRevalidatingAuthenticationStateProvider(
    ILoggerFactory loggerFactory,
    IServiceScopeFactory scopeFactory)
    : RevalidatingServerAuthenticationStateProvider(loggerFactory)
{
    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(1);

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState,
        CancellationToken cancellationToken)
    {
        var sessionId = authenticationState.User.FindFirstValue(AuthClaimTypes.SessionId);
        if (string.IsNullOrEmpty(sessionId))
            return false;

        await using var scope = scopeFactory.CreateAsyncScope();
        var sessions = scope.ServiceProvider.GetRequiredService<TokenSessionManager>();

        try
        {
            return await sessions.GetValidTokensAsync(sessionId, cancellationToken: cancellationToken) is not null;
        }
        catch (HttpRequestException)
        {
            return true;
        }
    }
}
