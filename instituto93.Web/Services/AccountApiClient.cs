using System.Net.Http.Json;
using instituto93.Web.Auth;

namespace instituto93.Web.Services;

public sealed class AccountApiClient(HttpClient httpClient, AccessTokenProvider accessTokens)
    : AuthorizedApiClient(httpClient, accessTokens)
{
    public async Task<CurrentUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(() => new HttpRequestMessage(HttpMethod.Get, "api/Auth/me"), cancellationToken);
        if (response is null)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CurrentUser>(cancellationToken);
    }
}
