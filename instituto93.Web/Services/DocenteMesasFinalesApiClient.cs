using System.Net.Http.Json;
using System.Text.Json;
using instituto93.Web.Models;
using instituto93.Web.Auth;
using instituto93.Web.Mocks;

namespace instituto93.Web.Services;

public sealed class DocenteMesasFinalesApiClient(
    HttpClient httpClient,
    AccessTokenProvider accessTokenProvider,
    MockDataStore? mocks = null)
    : AuthorizedApiClient(httpClient, accessTokenProvider)
{
    private const string BasePath = "api/mesas-finales/docente";

    public async Task<MesasFinalesResult> GetMesasAsync(CancellationToken cancellationToken = default)
    {
        var result = await GetMesasFromApiAsync(cancellationToken);
        return mocks is not null && result.Mesas.Count == 0
            ? MesasFinalesResult.Success(mocks.MesasDocente())
            : result;
    }

    private async Task<MesasFinalesResult> GetMesasFromApiAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var response = await SendAuthorizedAsync(
                () => new HttpRequestMessage(HttpMethod.Get, BasePath),
                cancellationToken);
            if (!response.IsSuccessStatusCode)
                return MesasFinalesResult.Failure(await ReadErrorAsync(response, "No se pudieron cargar las mesas finales.", cancellationToken));

            var mesas = await response.Content.ReadFromJsonAsync<List<MesaFinalDocenteModel>>(cancellationToken);
            return MesasFinalesResult.Success(mesas ?? []);
        }
        catch (HttpRequestException)
        {
            return MesasFinalesResult.Failure("No se pudo conectar con el servicio de mesas finales.");
        }
        catch (TaskCanceledException)
        {
            return MesasFinalesResult.Failure("La carga demoró demasiado y se canceló.");
        }
        catch (JsonException)
        {
            return MesasFinalesResult.Failure("El servicio devolvió datos de mesas finales que no se pudieron interpretar.");
        }
        catch (InvalidOperationException)
        {
            return MesasFinalesResult.Failure("No se pudo obtener la sesión para consultar las mesas finales.");
        }
    }

    public async Task<MesaFinalDetalleResult> GetMesaAsync(int mesaId, CancellationToken cancellationToken = default)
    {
        if (mocks?.TieneMesa(mesaId) == true && mocks.MesaDocente(mesaId) is { } mock)
            return MesaFinalDetalleResult.Success(mock);

        try
        {
            using var response = await SendAuthorizedAsync(
                () => new HttpRequestMessage(HttpMethod.Get, $"{BasePath}/{mesaId}"),
                cancellationToken);
            if (!response.IsSuccessStatusCode)
                return MesaFinalDetalleResult.Failure(await ReadErrorAsync(response, "No se pudo cargar la mesa seleccionada.", cancellationToken));

            var mesa = await response.Content.ReadFromJsonAsync<MesaFinalDetalleModel>(cancellationToken);
            return mesa is null
                ? MesaFinalDetalleResult.Failure("La respuesta de la mesa está vacía.")
                : MesaFinalDetalleResult.Success(mesa);
        }
        catch (HttpRequestException)
        {
            return MesaFinalDetalleResult.Failure("No se pudo conectar con el servicio de mesas finales.");
        }
        catch (TaskCanceledException)
        {
            return MesaFinalDetalleResult.Failure("La carga demoró demasiado y se canceló.");
        }
        catch (JsonException)
        {
            return MesaFinalDetalleResult.Failure("El servicio devolvió datos de la mesa que no se pudieron interpretar.");
        }
        catch (InvalidOperationException)
        {
            return MesaFinalDetalleResult.Failure("No se pudo obtener la sesión para consultar la mesa.");
        }
    }

    public async Task<GuardarNotasResult> GuardarNotasAsync(
        int mesaId,
        GuardarNotasFinalesRequest request,
        CancellationToken cancellationToken = default)
    {
        if (mocks?.TieneMesa(mesaId) == true)
        {
            mocks.GuardarNotasFinales(mesaId, request);
            return GuardarNotasResult.Success();
        }

        try
        {
            using var response = await SendAuthorizedAsync(
                () => new HttpRequestMessage(HttpMethod.Put, $"{BasePath}/{mesaId}/notas")
                {
                    Content = JsonContent.Create(request)
                },
                cancellationToken);
            return response.IsSuccessStatusCode
                ? GuardarNotasResult.Success()
                : GuardarNotasResult.Failure(await ReadErrorAsync(response, "No se pudieron guardar las notas.", cancellationToken));
        }
        catch (HttpRequestException)
        {
            return GuardarNotasResult.Failure("No se pudo conectar con el servicio de mesas finales.");
        }
        catch (TaskCanceledException)
        {
            return GuardarNotasResult.Failure("El guardado demoró demasiado y se canceló.");
        }
        catch (JsonException)
        {
            return GuardarNotasResult.Failure("El servicio devolvió una respuesta que no se pudo interpretar.");
        }
        catch (InvalidOperationException)
        {
            return GuardarNotasResult.Failure("No se pudo obtener la sesión para guardar las notas.");
        }
    }

    private static async Task<string> ReadErrorAsync(
        HttpResponseMessage response,
        string fallback,
        CancellationToken cancellationToken)
    {
        var status = (int)response.StatusCode;
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(body))
        {
            try
            {
                var error = JsonSerializer.Deserialize<ApiError>(body, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                if (!string.IsNullOrWhiteSpace(error?.Message))
                    return error.Message;
            }
            catch (JsonException)
            {
            }
        }

        return $"{fallback} (HTTP {status}).";
    }

    private sealed record ApiError(string? Message);
}

public sealed record MesasFinalesResult(bool IsSuccess, List<MesaFinalDocenteModel> Mesas, string? Error)
{
    public static MesasFinalesResult Success(List<MesaFinalDocenteModel> mesas) => new(true, mesas, null);
    public static MesasFinalesResult Failure(string error) => new(false, [], error);
}

public sealed record MesaFinalDetalleResult(bool IsSuccess, MesaFinalDetalleModel? Mesa, string? Error)
{
    public static MesaFinalDetalleResult Success(MesaFinalDetalleModel mesa) => new(true, mesa, null);
    public static MesaFinalDetalleResult Failure(string error) => new(false, null, error);
}

public sealed record GuardarNotasResult(bool IsSuccess, string? Error)
{
    public static GuardarNotasResult Success() => new(true, null);
    public static GuardarNotasResult Failure(string error) => new(false, error);
}
