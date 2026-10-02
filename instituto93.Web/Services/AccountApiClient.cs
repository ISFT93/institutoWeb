using System.Net.Http.Json;
using instituto93.Web.Models;

namespace instituto93.Web.Services;

public sealed class AccountApiClient(HttpClient httpClient)
{
    public Task<CurrentUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default) =>
        httpClient.GetFromJsonAsync<CurrentUser>("api/Auth/me", cancellationToken);
}
