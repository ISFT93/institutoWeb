using System.Net.Http.Json;
using System.Text.Json;
using instituto93.Web.Models;

namespace instituto93.Web.Services;

public sealed class AuthApiClient(HttpClient httpClient)
{
    public async Task<LoginResult> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
    {
        var request = new LoginRequest(model.Dni, model.Password);
        using var response = await httpClient.PostAsJsonAsync("api/Auth/login", request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var payload = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
            return payload?.Token is not null
                ? LoginResult.Success(payload.Token)
                : LoginResult.Failure("La API no devolvió un token de acceso.");
        }

        return LoginResult.Failure(await ReadErrorAsync(response, "No fue posible iniciar sesión.", cancellationToken));
    }

    public async Task<DniCheckResult> CheckDniAsync(string dni, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("api/Auth/dni-status", new DniStatusRequest(dni), cancellationToken);

        if (!response.IsSuccessStatusCode)
            return DniCheckResult.Failure(await ReadErrorAsync(response, "No fue posible verificar el D.N.I.", cancellationToken));

        var payload = await response.Content.ReadFromJsonAsync<DniStatusResponse>(cancellationToken);
        return payload?.Estado switch
        {
            "ConContrasena" => DniCheckResult.Ok(DniAccess.HasPassword),
            "SinContrasena" => DniCheckResult.Ok(DniAccess.NeedsPassword),
            _ => DniCheckResult.Failure("La API devolvió una respuesta inválida.")
        };
    }

    public async Task<ApiResult> CreatePasswordAsync(string dni, CreatePasswordModel model, CancellationToken cancellationToken = default)
    {
        var request = new CreatePasswordRequest(dni, model.Password, model.ConfirmPassword);
        using var response = await httpClient.PostAsJsonAsync("api/Auth/create-password", request, cancellationToken);

        return response.IsSuccessStatusCode
            ? ApiResult.Success()
            : ApiResult.Failure(await ReadErrorAsync(response, "No fue posible crear la contraseña.", cancellationToken));
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response, string fallback, CancellationToken cancellationToken)
    {
        try
        {
            var error = await response.Content.ReadFromJsonAsync<ApiError>(cancellationToken);
            return string.IsNullOrWhiteSpace(error?.Message) ? fallback : error.Message;
        }
        catch (JsonException)
        {
            return fallback;
        }
    }

    private sealed record LoginRequest(string Dni, string Password);
    private sealed record LoginResponse(string Token);
    private sealed record DniStatusRequest(string Dni);
    private sealed record DniStatusResponse(string Estado);
    private sealed record CreatePasswordRequest(string Dni, string Password, string ConfirmPassword);
    private sealed record ApiError(string? Message);
}

public sealed record LoginResult(bool IsSuccess, string? Token, string? Error)
{
    public static LoginResult Success(string token) => new(true, token, null);
    public static LoginResult Failure(string error) => new(false, null, error);
}

public enum DniAccess
{
    HasPassword,
    NeedsPassword
}

public sealed record DniCheckResult(DniAccess? Access, string? Error)
{
    public bool IsSuccess => Access is not null;
    public static DniCheckResult Ok(DniAccess access) => new(access, null);
    public static DniCheckResult Failure(string error) => new(null, error);
}

public sealed record ApiResult(bool IsSuccess, string? Error)
{
    public static ApiResult Success() => new(true, null);
    public static ApiResult Failure(string error) => new(false, error);
}
