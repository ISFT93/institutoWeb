namespace instituto93.Domain.DTOs;

public sealed record MateriaDocenteDto(int CursadaId, string Materia, string Carrera,
    string Curso, int AnioLectivo, int CantidadEstudiantes, int CantidadEvaluaciones);

public sealed record MateriaDocenteDetalleDto(int CursadaId, string Materia, string Carrera,
    string Curso, int AnioLectivo, int? CargaHoraria, List<EstudianteMateriaDto> Estudiantes);

public sealed record EstudianteMateriaDto(int CursadaAlumnoCarreraId, int AlumnoId,
    string NombreCompleto, string Dni, string Estado, decimal? PorcentajeAsistencia);

public sealed record EvaluacionDocenteDto(int EvaluacionId, int TipoEvaluacionId, string Nombre, string Tipo,
    DateTime Fecha, byte Cuatrimestre, string Estado, decimal? Ponderacion, string Version,
    int NotasCargadas, int CantidadEstudiantes);
public sealed record CrearEvaluacionDto(int TipoEvaluacionId, string? Nombre, DateTime Fecha,
    byte Cuatrimestre);
public sealed record ActualizarEvaluacionDto(int TipoEvaluacionId, string? Nombre, DateTime Fecha,
    byte Cuatrimestre, string? Estado, decimal? Ponderacion, string? Version);
public sealed record TipoEvaluacionDto(int TipoEvaluacionId, string Codigo, string Descripcion);
public sealed record EvaluacionNotasDto(int EvaluacionId, int CursadaId, string Nombre, string Tipo,
    DateTime Fecha, byte Cuatrimestre, string Estado, decimal NotaMinima, decimal NotaMaxima,
    decimal NotaAprobacion, List<NotaAlumnoDto> Estudiantes);
public sealed record NotaAlumnoDto(int CursadaAlumnoCarreraId, int AlumnoId, string NombreCompleto,
    string Dni, decimal? Nota, bool Ausente, string? Observaciones, string? Version);
public sealed record GuardarNotaAlumnoDto(int CursadaAlumnoCarreraId, decimal? Nota, bool Ausente,
    string? Observaciones, string? Version);
public sealed record GuardarNotasEvaluacionDto(List<GuardarNotaAlumnoDto>? Notas);
