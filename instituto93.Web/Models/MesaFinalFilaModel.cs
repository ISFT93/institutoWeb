namespace instituto93.Web.Models;

public sealed record MesaFinalFilaModel(
    int MesaFinalId,
    string Materia,
    int AnioCarrera,
    string Asiste,
    string Docente,
    DateTime? Fecha,
    string Sede,
    bool EstaInscripto);
