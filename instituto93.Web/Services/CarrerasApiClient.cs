using System.Net.Http.Json;
using System.Text.Json;
using instituto93.Web.Models;

namespace instituto93.Web.Services;

public sealed class CarrerasApiClient(HttpClient httpClient)
{
    public async Task<CarrerasResult> GetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await httpClient.GetAsync("Carreras/lookup", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var payload =
                    await response.Content.ReadFromJsonAsync<List<CarrerasLookupModel>>(cancellationToken);

                return CarrerasResult.Success(payload ?? []);
            }

            return CarrerasResult.Failure(
                await ReadErrorAsync(response, "No se pudieron cargar las carreras.", cancellationToken));
        }
        catch (HttpRequestException ex)
        {
            return CarrerasResult.Failure(
                "No se pudo conectar con instituto93.Controller. Verificá que la API esté iniciada. " + ex.Message);
        }
        catch (TaskCanceledException)
        {
            return CarrerasResult.Failure("La carga de las carreras demoró demasiado y se canceló.");
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

public sealed record CarrerasResult(bool IsSuccess, List<CarrerasLookupModel> Carreras, string? Error)
{
    public static CarrerasResult Success(List<CarrerasLookupModel> carreras) => new(true, carreras, null);

    public static CarrerasResult Failure(string error) => new(false, [], error);
}
