namespace instituto93.Domain.Models
{
    // Fila del listado de mesas finales: materia, mesa examinadora y sede ya resueltos por joins.
    public sealed record MesaFinalFila(
        int MesaFinalId,
        string Materia,
        int AnioCarrera,
        string Asiste,
        string Docente,
        DateTime? Fecha,
        string Sede);
}
