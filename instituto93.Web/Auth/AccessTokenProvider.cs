using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace instituto93.Web.Auth;

// Scoped: funciona tanto en el prerender (request HTTP) como dentro del circuito interactivo.
public sealed class AccessTokenProvider(
    AuthenticationStateProvider authenticationStateProvider,
    TokenSessionManager sessions,
    NavigationManager navigation)
{
    /// <summary>
    /// Devuelve un access token vigente o null si la sesión terminó (en ese caso ya redirigió a /login).
    /// </summary>
    public async Task<string?> GetAccessTokenAsync(bool forceRefresh = false, CancellationToken cancellationToken = default)
    {
        var state = await authenticationStateProvider.GetAuthenticationStateAsync();
        var sessionId = state.User.FindFirstValue(AuthClaimTypes.SessionId);

        var tokens = string.IsNullOrEmpty(sessionId)
            ? null
            : await sessions.GetValidTokensAsync(sessionId, forceRefresh, cancellationToken);

        if (tokens is null)
        {
            SessionExpired();
            return null;
        }

        return tokens.AccessToken;
    }

    public void SessionExpired() => LoginRedirect.Navigate(navigation);
}
