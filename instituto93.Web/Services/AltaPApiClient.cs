using System.Net.Http.Json;
using System.Text.Json;
using instituto93.Web.Models;

namespace instituto93.Web.Services;

public sealed class AltaPApiClient(HttpClient httpClient)
{
    public async Task<AltaPResult> CreateAsync(AltaPModel model, CancellationToken cancellationToken = default)
    {
        var request = ToRequest(model);

        using var response = await httpClient.PostAsJsonAsync("Alumno/Alta provisoria", request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var payload = await response.Content.ReadFromJsonAsync<CreatedResponse>(cancellationToken);
            return AltaPResult.Success(payload?.AlumnoId);
        }

        return AltaPResult.Failure(await ReadErrorAsync(response, "No se pudo realizar el alta provisoria.", cancellationToken));
    }
    private static AltaPRequest ToRequest(AltaPModel model) =>
        new()
        {
            CarreraId = model.CarreraId,
            Apellido = model.Apellido,
            Nombre = model.Nombre,
            TipoDocumento = model.TipoDocumento,
            NumeroDocumento = model.NumeroDocumento,
            Sexo = model.Sexo,
            FechaNacimiento = model.FechaNacimiento,
            Calle = model.Calle,
            Localidad = model.Localidad,
            Telefono = model.Telefono,
            Email = model.Email,
            Password=model.Password,
            ConfirmarPassword=model.ConfirmarPassword
        };

    /* La API puede devolver { message, detail } o, en Development, el HTML/text de la pagina de excepcion.
    Si no es JSON legible se devuelve el status y un fragmento del cuerpo para poder diagnosticar. */
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
            var error = JsonSerializer.Deserialize<ApiError>(body, ErrorJson);

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

    private static readonly JsonSerializerOptions ErrorJson = new(JsonSerializerDefaults.Web);

    private sealed record CreatedResponse(int? AlumnoId);

    private sealed record ApiError(string? Message, string? Detail);

    private sealed class AltaPRequest
    {
        public int? CarreraId { get; set; }

        public string Apellido { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string TipoDocumento { get; set; } = string.Empty;

        public string NumeroDocumento { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;

        public DateTime? FechaNacimiento { get; set; }

        public string Calle { get; set; } = string.Empty;

        public string Localidad { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}

public sealed record AltaPResult(bool IsSuccess, int? AlumnoId, string? Error)
{
    public static AltaPResult Success(int? alumnoId) => new(true, alumnoId, null);

    public static AltaPResult Failure(string error) => new(false, null, error);
}