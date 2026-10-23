namespace instituto93.Web.Models;
public sealed record MateriaDocenteModel(int CursadaId,string Materia,string Carrera,string Curso,int AnioLectivo,int CantidadEstudiantes,int CantidadEvaluaciones);
public sealed record MateriaDocenteDetalleModel(int CursadaId,string Materia,string Carrera,string Curso,int AnioLectivo,int? CargaHoraria,List<EstudianteMateriaModel> Estudiantes);
public sealed record EstudianteMateriaModel(int CursadaAlumnoCarreraId,int AlumnoId,string NombreCompleto,string Dni,string Estado,decimal? PorcentajeAsistencia);
public sealed record EvaluacionDocenteModel(int EvaluacionId,int TipoEvaluacionId,string Nombre,string Tipo,DateTime Fecha,byte Cuatrimestre,string Estado,decimal? Ponderacion,string Version,int NotasCargadas,int CantidadEstudiantes);
public sealed record TipoEvaluacionModel(int TipoEvaluacionId,string Codigo,string Descripcion);
public sealed record CrearEvaluacionRequest(int TipoEvaluacionId,string Nombre,DateTime Fecha,byte Cuatrimestre);
public sealed record ActualizarEvaluacionRequest(int TipoEvaluacionId,string Nombre,DateTime Fecha,byte Cuatrimestre,string Estado,decimal? Ponderacion,string Version);
public sealed record EvaluacionNotasModel(int EvaluacionId,int CursadaId,string Nombre,string Tipo,DateTime Fecha,byte Cuatrimestre,string Estado,decimal NotaMinima,decimal NotaMaxima,decimal NotaAprobacion,List<NotaAlumnoModel> Estudiantes);
public sealed class NotaAlumnoModel{public int CursadaAlumnoCarreraId{get;init;}public int AlumnoId{get;init;}public string NombreCompleto{get;init;}="";public string Dni{get;init;}="";public decimal? Nota{get;set;}public bool Ausente{get;set;}public string? Observaciones{get;set;}public string? Version{get;init;}}
public sealed record GuardarNotaAlumnoRequest(int CursadaAlumnoCarreraId,decimal? Nota,bool Ausente,string? Observaciones,string? Version);
public sealed record GuardarNotasEvaluacionRequest(List<GuardarNotaAlumnoRequest> Notas);
