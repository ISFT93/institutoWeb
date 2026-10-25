namespace instituto93.Domain.DTOs;

public sealed record MesaFinalDocenteDto(
    int MesaId,
    string Materia,
    string Carrera,
    DateTime FechaHora,
    string Turno,
    string Estado,
    int CantidadEstudiantes,
    string? Aula);

public sealed record MesaFinalDetalleDto(
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
    List<EstudianteNotaFinalDto> Estudiantes);

public sealed record EstudianteNotaFinalDto(
    int MesaAlumnoId,
    int AlumnoId,
    string NombreCompleto,
    string Dni,
    decimal? Nota,
    bool Ausente,
    DateTime UltimaModificacion);

public sealed record GuardarNotaFinalDto(
    int MesaAlumnoId,
    decimal? Nota,
    bool Ausente,
    DateTime UltimaModificacion);

public sealed record GuardarNotasFinalesDto(List<GuardarNotaFinalDto>? Notas);
