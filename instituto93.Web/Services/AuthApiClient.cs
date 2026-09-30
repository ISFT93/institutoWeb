using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using instituto93.Web.Auth;
using instituto93.Web.Models;

namespace instituto93.Web.Services;

public sealed class AuthApiClient(HttpClient httpClient)
{
    public async Task<LoginResult> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
    {
        var request = new LoginRequest(model.Dni, model.Password);
        using var response = await httpClient.PostAsJsonAsync("api/Auth/login", request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return LoginResult.Failure(await ReadErrorAsync(response, "No fue posible iniciar sesión.", cancellationToken));

        var tokens = ToTokenSet(await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken));
        if (tokens is null)
            return LoginResult.Failure("La API no devolvió un token de acceso.");

        var user = await GetCurrentUserAsync(tokens.AccessToken, cancellationToken);
        return user is null
            ? LoginResult.Failure("La API no devolvió los datos del usuario.")
            : LoginResult.Success(new LoginTicket(tokens, user));
    }

    // Devuelve null si la API rechazó el refresh token (401/400). Lanza HttpRequestException
    // ante errores de red o del servidor, que no deben cerrar la sesión.
    public async Task<TokenSet?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await httpClient.PostAsJsonAsync(
                "api/Auth/refresh",
                new RefreshTokenRequest(refreshToken),
                cancellationToken);

            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.BadRequest)
                return null;

            response.EnsureSuccessStatusCode();
            return ToTokenSet(await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken))
                ?? throw new HttpRequestException("La API devolvió una respuesta de refresh inválida.");
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            throw new HttpRequestException("La API no respondió a tiempo.", ex);
        }
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "api/Auth/logout",
            new RefreshTokenRequest(refreshToken),
            cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private async Task<CurrentUser?> GetCurrentUserAsync(string accessToken, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/Auth/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        using var response = await httpClient.SendAsync(request, cancellationToken);

        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<CurrentUser>(cancellationToken)
            : null;
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

    private static TokenSet? ToTokenSet(TokenResponse? response) =>
        string.IsNullOrWhiteSpace(response?.AccessToken) || string.IsNullOrWhiteSpace(response.RefreshToken)
            ? null
            : new TokenSet(
                response.AccessToken,
                response.AccessTokenExpiresAt,
                response.RefreshToken,
                response.RefreshTokenExpiresAt);

    private sealed record LoginRequest(string Dni, string Password);
    private sealed record RefreshTokenRequest(string RefreshToken);
    private sealed record TokenResponse(
        string? AccessToken,
        DateTimeOffset AccessTokenExpiresAt,
        string? RefreshToken,
        DateTimeOffset RefreshTokenExpiresAt);
    private sealed record DniStatusRequest(string Dni);
    private sealed record DniStatusResponse(string Estado);
    private sealed record CreatePasswordRequest(string Dni, string Password, string ConfirmPassword);
    private sealed record ApiError(string? Message);
}

public sealed record LoginResult(LoginTicket? Ticket, string? Error)
{
    public bool IsSuccess => Ticket is not null;
    public static LoginResult Success(LoginTicket ticket) => new(ticket, null);
    public static LoginResult Failure(string error) => new(null, error);
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
