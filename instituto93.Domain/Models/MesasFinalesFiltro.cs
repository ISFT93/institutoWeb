namespace instituto93.Domain.Models
{
    // Filtros opcionales del listado de mesas finales.
    public sealed record MesasFinalesFiltro(
        int? CarreraId,
        int? TurnoId,
        int? AnioCarreraId,
        int? AnioLectivo,
        string? Texto,
        int? AlumnoId);
}
