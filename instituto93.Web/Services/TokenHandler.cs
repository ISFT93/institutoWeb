using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using instituto93.Web.Auth;

namespace instituto93.Web.Services;

// Adjunta el access token guardado en la cookie a las llamadas a la API. Dentro del circuito,
// IHttpContextAccessor devuelve el HttpContext capturado al iniciar la conexión de SignalR.
public sealed class TokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext not available.");

        var accessToken = await httpContext.GetTokenAsync(AuthCookie.AccessToken)
            ?? throw new InvalidOperationException("No access token.");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
