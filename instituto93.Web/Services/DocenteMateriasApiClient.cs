using System.Net.Http.Json;
using instituto93.Web.Auth;
using instituto93.Web.Models;

namespace instituto93.Web.Services;

public sealed class DocenteMateriasApiClient(HttpClient client, AccessTokenProvider tokens)
    : AuthorizedApiClient(client, tokens)
{
    private const string Path = "api/materias/docente";

    public async Task<(List<MateriaDocenteModel> Data, string? Error)> GetAsync(CancellationToken ct = default)
    {
        try
        {
            using var r = await SendAuthorizedAsync(() => new(HttpMethod.Get, Path), ct);
            return r.IsSuccessStatusCode
                ? (await r.Content.ReadFromJsonAsync<List<MateriaDocenteModel>>(ct) ?? [], null)
                : ([], $"No se pudieron cargar las materias (HTTP {(int)r.StatusCode}).");
        }
        catch (Exception e) when (e is HttpRequestException or TaskCanceledException or InvalidOperationException)
        {
            return ([], "No se pudo conectar con el servicio de materias.");
        }
    }

    public async Task<(MateriaDocenteDetalleModel? Data, string? Error)> GetAsync(int id, CancellationToken ct = default)
    {
        try
        {
            using var r = await SendAuthorizedAsync(() => new(HttpMethod.Get, $"{Path}/{id}"), ct);
            return r.IsSuccessStatusCode
                ? (await r.Content.ReadFromJsonAsync<MateriaDocenteDetalleModel>(ct), null)
                : (null, $"No se pudo cargar la materia (HTTP {(int)r.StatusCode}).");
        }
        catch (Exception e) when (e is HttpRequestException or TaskCanceledException or InvalidOperationException)
        {
            return (null, "No se pudo conectar con el servicio de materias.");
        }
    }

    public async Task<List<EvaluacionDocenteModel>> GetEvaluacionesAsync(int id, CancellationToken ct = default)
    {
        using var r = await SendAuthorizedAsync(() => new(HttpMethod.Get, $"{Path}/{id}/evaluaciones"), ct);
        r.EnsureSuccessStatusCode();
        return await r.Content.ReadFromJsonAsync<List<EvaluacionDocenteModel>>(ct) ?? [];
    }

    public async Task<List<TipoEvaluacionModel>> GetTiposAsync(CancellationToken ct = default)
    {
        using var r = await SendAuthorizedAsync(() => new(HttpMethod.Get, $"{Path}/tipos-evaluacion"), ct);
        r.EnsureSuccessStatusCode();
        return await r.Content.ReadFromJsonAsync<List<TipoEvaluacionModel>>(ct) ?? [];
    }

    public async Task CrearAsync(int id, CrearEvaluacionRequest request, CancellationToken ct = default)
    {
        using var r = await SendAuthorizedAsync(
            () => new(HttpMethod.Post, $"{Path}/{id}/evaluaciones") { Content = JsonContent.Create(request) }, ct);
        r.EnsureSuccessStatusCode();
    }

    public async Task ActualizarAsync(int id, ActualizarEvaluacionRequest request, CancellationToken ct = default)
    {
        using var r = await SendAuthorizedAsync(
            () => new(HttpMethod.Put, $"{Path}/evaluaciones/{id}") { Content = JsonContent.Create(request) }, ct);
        r.EnsureSuccessStatusCode();
    }

    public async Task<EvaluacionNotasModel?> GetNotasAsync(int id, CancellationToken ct = default)
    {
        using var r = await SendAuthorizedAsync(() => new(HttpMethod.Get, $"{Path}/evaluaciones/{id}"), ct);
        r.EnsureSuccessStatusCode();
        return await r.Content.ReadFromJsonAsync<EvaluacionNotasModel>(ct);
    }

    public async Task GuardarAsync(int id, GuardarNotasEvaluacionRequest request, CancellationToken ct = default)
    {
        using var r = await SendAuthorizedAsync(
            () => new(HttpMethod.Put, $"{Path}/evaluaciones/{id}/notas") { Content = JsonContent.Create(request) }, ct);
        r.EnsureSuccessStatusCode();
    }
}
