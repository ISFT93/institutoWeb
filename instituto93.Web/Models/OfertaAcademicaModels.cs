namespace instituto93.Web.Models;

public sealed record MateriaInscripcionModel(
    int MateriaId, string Carrera, int Anio, string Materia, string Docente, string Sede);

public sealed record MesaFinalInscripcionModel(
    int MesaId, string Carrera, int Anio, string Materia, string Docente, DateTime FechaHora, string Sede, string Turno);
