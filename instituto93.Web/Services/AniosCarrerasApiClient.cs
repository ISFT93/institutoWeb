using System.Net.Http.Json;
using System.Text.Json;
using instituto93.Web.Models;

namespace instituto93.Web.Services;

public sealed class AniosCarrerasApiClient(HttpClient httpClient)
{
    public async Task<AniosCarrerasResult> GetAsync(int carreraId, CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await httpClient.GetAsync(
                $"AniosCarreras/lookup?carreraId={carreraId}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var payload =
                    await response.Content.ReadFromJsonAsync<List<AnioCarreraLookupModel>>(cancellationToken);

                return AniosCarrerasResult.Success(payload ?? []);
            }

            return AniosCarrerasResult.Failure(
                await ReadErrorAsync(response, "No se pudieron cargar los años de la carrera.", cancellationToken));
        }
        catch (HttpRequestException ex)
        {
            return AniosCarrerasResult.Failure(
                "No se pudo conectar con instituto93.Controller. Verificá que la API esté iniciada. " + ex.Message);
        }
        catch (TaskCanceledException)
        {
            return AniosCarrerasResult.Failure("La carga de los años de la carrera demoró demasiado y se canceló.");
        }
    }

    private static async Task<string> ReadErrorAsync(
        HttpResponseMessage response,
        string fallback,
        CancellationToken cancellationToken)
    {
        var status = (int)response.StatusCode;
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(body))
            return $"{fallback} (HTTP {status} {response.ReasonPhrase}).";

        try
        {
            var error = JsonSerializer.Deserialize<ApiError>(body, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            if (error is not null && !string.IsNullOrWhiteSpace(error.Message))
                return string.IsNullOrWhiteSpace(error.Detail)
                    ? $"{error.Message} (HTTP {status})"
                    : $"{error.Message} {error.Detail}";
        }
        catch (JsonException)
        {
        }

        var detalle = body.Length > 300 ? string.Concat(body[..300], "...") : body;

        return $"HTTP {status} {response.ReasonPhrase}: {detalle}";
    }

    private sealed record ApiError(string? Message, string? Detail);
}

public sealed record AniosCarrerasResult(bool IsSuccess, List<AnioCarreraLookupModel> Anios, string? Error)
{
    public static AniosCarrerasResult Success(List<AnioCarreraLookupModel> anios) => new(true, anios, null);

    public static AniosCarrerasResult Failure(string error) => new(false, [], error);
}
