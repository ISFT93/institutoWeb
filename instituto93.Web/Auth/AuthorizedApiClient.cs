using System.Net;
using System.Net.Http.Headers;

namespace instituto93.Web.Auth;

// Base para los clientes de endpoints protegidos de la API. Agrega el Bearer token y,
// si la API responde 401, fuerza una renovación y reintenta una vez.
// No se usa un DelegatingHandler porque IHttpClientFactory crea los handlers en otro scope
// de DI y no tendrían acceso a la sesión del circuito.
public abstract class AuthorizedApiClient(HttpClient httpClient, AccessTokenProvider accessTokens)
{
    /// <summary>
    /// Devuelve null si la sesión expiró; la redirección a /login ya quedó en curso.
    /// </summary>
    protected async Task<HttpResponseMessage?> SendAsync(
        Func<HttpRequestMessage> createRequest,
        CancellationToken cancellationToken = default)
    {
        var accessToken = await accessTokens.GetAccessTokenAsync(cancellationToken: cancellationToken);
        if (accessToken is null)
            return null;

        var response = await SendWithTokenAsync(createRequest(), accessToken, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        response.Dispose();
        accessToken = await accessTokens.GetAccessTokenAsync(forceRefresh: true, cancellationToken);
        if (accessToken is null)
            return null;

        response = await SendWithTokenAsync(createRequest(), accessToken, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        response.Dispose();
        accessTokens.SessionExpired();
        return null;
    }

    private async Task<HttpResponseMessage> SendWithTokenAsync(
        HttpRequestMessage request,
        string accessToken,
        CancellationToken cancellationToken)
    {
        using (request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return await httpClient.SendAsync(request, cancellationToken);
        }
    }
}
