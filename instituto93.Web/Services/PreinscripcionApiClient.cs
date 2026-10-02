using System.Net.Http.Json;
using System.Text.Json;
using instituto93.Web.Models;

namespace instituto93.Web.Services;

public sealed class PreinscripcionApiClient(HttpClient httpClient)
{
    public async Task<PreinscripcionResult> CreateAsync(
        PreinscripcionModel model,
        CancellationToken cancellationToken = default)
    {
        var request = ToRequest(model);

        using var response = await httpClient.PostAsJsonAsync(
            "Alumno/preinscripcion",
            request,
            cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var payload = await response.Content.ReadFromJsonAsync<CreatedResponse>(cancellationToken);
            return PreinscripcionResult.Success(payload?.AlumnoId);
        }

        return PreinscripcionResult.Failure(
            await ReadErrorAsync(response, "No se pudo guardar la preinscripción.", cancellationToken));
    }

    // Los adjuntos viajan como bool (adjuntado / no) porque IBrowserFile no es serializable a la API.
    // MateriasAdeuda no se manda: la API lo deriva de CantidadAdeudaMaterias.
    private static PreinscripcionRequest ToRequest(PreinscripcionModel model) =>
        new()
        {
            Carrera = model.Carrera,
            Apellido = model.Apellido,
            Nombre = model.Nombre,
            TipoDocumento = model.TipoDocumento,
            NumeroDocumento = model.NumeroDocumento,

            EstadoCivil = model.EstadoCivil,
            Sexo = model.Sexo,

            FechaNacimiento = model.FechaNacimiento,

            LocalidadNacimiento = model.LocalidadNacimiento,
            PaisNacimiento = model.PaisNacimiento,

            Calle = model.Calle,
            Numero = model.Numero,
            Piso = model.Piso,
            Departamento = model.Departamento,

            Provincia = model.Provincia,
            Distrito = model.Distrito,
            Localidad = model.Localidad,
            CodigoPostal = model.CodigoPostal,

            Telefono = model.Telefono,
            Celular = model.Celular,
            Email = model.Email,

            TituloSecundario = model.TituloSecundario,

            DescripcionMaterias = model.DescripcionMaterias,

            Titulo = model.Titulo,
            Orientacion = model.Orientacion,
            OtorgadoPor = model.OtorgadoPor,

            AnioEgreso = model.AnioEgreso,
            Promedio = model.Promedio,

            TituloTramite = model.TituloTramite,

            MayorTitulo = model.MayorTitulo,
            OtroTitulo = model.OtroTitulo,

            MayorOtorgadoPor = model.MayorOtorgadoPor,

            MayorPromedio = model.MayorPromedio,

            FotocopiaTitulo = model.FotocopiaTitulo is not null,

            ConstanciaTituloTramite = model.ConstanciaTituloTramite is not null,

            ConstanciaAdeudaMaterias = model.ConstanciaAdeudaMaterias is not null,

            CantidadAdeudaMaterias = model.CantidadAdeudaMaterias,

            CertificadoAptitud = model.CertificadoAptitud is not null,

            FotocopiaDocumento = model.FotocopiaDocumento is not null,

            FotoCarnet = model.FotoCarnet is not null,

            FotocopiaPartidaNacimiento = model.FotocopiaPartidaNacimiento is not null,

            VacunaAntihepatitis = model.VacunaAntihepatitis is not null,

            VacunaAntitetanica = model.VacunaAntitetanica is not null,

            Recibo = model.Recibo,

            Monto = model.Monto,

            ObraSocialPrepaga = model.ObraSocialPrepaga,

            DescripcionObraSocial = model.DescripcionObraSocial,

            TratamientoMedico = model.TratamientoMedico,

            DescripcionTratamiento = model.DescripcionTratamiento,

            Medicacion = model.Medicacion,

            DescripcionMedicacion = model.DescripcionMedicacion,

            Discapacidad = model.Discapacidad,

            DescripcionDiscapacidad = model.DescripcionDiscapacidad,

            EstadoDiscapacidad = model.EstadoDiscapacidad,

            CertificadoDiscapacidad = model.CertificadoDiscapacidad is not null,

            ContactoEmergencia = model.ContactoEmergencia,

            TelefonoContacto = model.TelefonoContacto
        };

    // La API puede devolver { message, detail } o, en Development, el HTML/text de la pagina de excepcion.
    // Si no es JSON legible se devuelve el status y un fragmento del cuerpo para poder diagnosticar.
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

    private sealed class PreinscripcionRequest
    {
        public string Carrera { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string TipoDocumento { get; set; } = string.Empty;

        public string NumeroDocumento { get; set; } = string.Empty;

        public string? EstadoCivil { get; set; }

        public string Sexo { get; set; } = string.Empty;

        public DateTime? FechaNacimiento { get; set; }

        public string LocalidadNacimiento { get; set; } = string.Empty;

        public string PaisNacimiento { get; set; } = string.Empty;

        public string Calle { get; set; } = string.Empty;

        public string? Numero { get; set; }

        public string? Piso { get; set; }

        public string? Departamento { get; set; }

        public string? Provincia { get; set; }

        public string? Distrito { get; set; }

        public string Localidad { get; set; } = string.Empty;

        public string? CodigoPostal { get; set; }

        public string? Telefono { get; set; }

        public string? Celular { get; set; }

        public string Email { get; set; } = string.Empty;

        public bool TituloSecundario { get; set; }

        public string? DescripcionMaterias { get; set; }

        public string? Titulo { get; set; }

        public string? Orientacion { get; set; }

        public string? OtorgadoPor { get; set; }

        public int? AnioEgreso { get; set; }

        public decimal? Promedio { get; set; }

        public bool TituloTramite { get; set; }

        public string? MayorTitulo { get; set; }

        public string? OtroTitulo { get; set; }

        public string? MayorOtorgadoPor { get; set; }

        public decimal? MayorPromedio { get; set; }

        public bool FotocopiaTitulo { get; set; }

        public bool ConstanciaTituloTramite { get; set; }

        public bool ConstanciaAdeudaMaterias { get; set; }

        public int? CantidadAdeudaMaterias { get; set; }

        public bool CertificadoAptitud { get; set; }

        public bool FotocopiaDocumento { get; set; }

        public bool FotoCarnet { get; set; }

        public bool FotocopiaPartidaNacimiento { get; set; }

        public bool VacunaAntihepatitis { get; set; }

        public bool VacunaAntitetanica { get; set; }

        public int? Recibo { get; set; }

        public int? Monto { get; set; }

        public bool ObraSocialPrepaga { get; set; }

        public string? DescripcionObraSocial { get; set; }

        public bool TratamientoMedico { get; set; }

        public string? DescripcionTratamiento { get; set; }

        public bool Medicacion { get; set; }

        public string? DescripcionMedicacion { get; set; }

        public bool Discapacidad { get; set; }

        public string? DescripcionDiscapacidad { get; set; }

        public string? EstadoDiscapacidad { get; set; }

        public bool CertificadoDiscapacidad { get; set; }

        public string? ContactoEmergencia { get; set; }

        public string? TelefonoContacto { get; set; }
    }
}

public sealed record PreinscripcionResult(bool IsSuccess, int? AlumnoId, string? Error)
{
    public static PreinscripcionResult Success(int? alumnoId) => new(true, alumnoId, null);

    public static PreinscripcionResult Failure(string error) => new(false, null, error);
}