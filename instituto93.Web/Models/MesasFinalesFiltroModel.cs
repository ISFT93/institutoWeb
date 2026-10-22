namespace instituto93.Web.Models;

public sealed record MesasFinalesFiltroModel(
    int? CarreraId,
    int? TurnoId,
    int? AnioCarreraId,
    int? AnioLectivo,
    string? Texto);
