using System.Net;
using System.Net.Http.Headers;
using instituto93.Web.Auth;

namespace instituto93.Web.Services;

public abstract class AuthorizedApiClient(HttpClient httpClient, AccessTokenProvider accessTokenProvider)
{
    protected async Task<HttpResponseMessage> SendAuthorizedAsync(
        Func<HttpRequestMessage> requestFactory,
        CancellationToken cancellationToken = default)
    {
        var accessToken = await accessTokenProvider.GetAsync();
        var response = await SendAsync(requestFactory, accessToken, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        var currentAccessToken = await accessTokenProvider.GetAsync();
        if (currentAccessToken == accessToken)
            return response;

        response.Dispose();
        return await SendAsync(requestFactory, currentAccessToken, cancellationToken);
    }

    private async Task<HttpResponseMessage> SendAsync(
        Func<HttpRequestMessage> requestFactory,
        string accessToken,
        CancellationToken cancellationToken)
    {
        using var request = requestFactory();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return await httpClient.SendAsync(request, cancellationToken);
    }
}
