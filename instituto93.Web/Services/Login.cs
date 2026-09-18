using System.Net.Http.Json;
using instituto93.Web.Models;

namespace instituto93.Web.Services;

public sealed class AuthApiClient(HttpClient httpClient)
{
    public async Task<LoginResult> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
    {
        var request = new LoginRequest(model.EmailOrDni, model.Password);
        using var response = await httpClient.PostAsJsonAsync("api/Auth/login", request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var payload = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
            return payload?.Token is not null
                ? LoginResult.Success(payload.Token)
                : LoginResult.Failure("La API no devolvió un token de acceso.");
        }

        var error = await response.Content.ReadFromJsonAsync<ApiError>(cancellationToken);
        return LoginResult.Failure(error?.Message ?? "No fue posible iniciar sesión.");
    }

    private sealed record LoginRequest(string EmailOrDni, string Password);
    private sealed record LoginResponse(string Token);
    private sealed record ApiError(string Message);
}

public sealed record LoginResult(bool IsSuccess, string? Token, string? Error)
{
    public static LoginResult Success(string token) => new(true, token, null);
    public static LoginResult Failure(string error) => new(false, null, error);
}
