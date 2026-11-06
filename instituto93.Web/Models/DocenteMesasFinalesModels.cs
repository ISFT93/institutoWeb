namespace instituto93.Web.Models;

public sealed record MesaFinalDocenteModel(
    int MesaId,
    string Materia,
    string Carrera,
    DateTime FechaHora,
    string Turno,
    string Estado,
    int CantidadEstudiantes,
    string? Aula);

public sealed record MesaFinalDetalleModel(
    int MesaId,
    string Materia,
    string Carrera,
    DateTime FechaHora,
    string Turno,
    string Estado,
    string? Aula,
    decimal NotaMinima,
    decimal NotaMaxima,
    decimal NotaAprobacion,
    List<EstudianteNotaFinalModel> Estudiantes);

public sealed class EstudianteNotaFinalModel
{
    public int MesaAlumnoId { get; init; }
    public int AlumnoId { get; init; }
    public string NombreCompleto { get; init; } = string.Empty;
    public string Dni { get; init; } = string.Empty;

    public decimal? Nota { get; set; }
    public DateTime UltimaModificacion { get; init; }
}

public sealed record GuardarNotaFinalModel(
    int MesaAlumnoId,
    decimal? Nota,
    DateTime UltimaModificacion);

public sealed record GuardarNotasFinalesRequest(List<GuardarNotaFinalModel> Notas);
