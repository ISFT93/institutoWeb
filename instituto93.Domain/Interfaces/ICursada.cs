using System;

namespace instituto93.Domain.Interfaces
{
    public interface ICursada
    {
        int CursadaId { get; set; }
        int CursoMateriaId { get; set; }
        string AnioLectivo { get; set; }
        string Anio { get; set; }
        int CantidadAlumnos { get; set; }
        int CantidadAlumnosRecursantes { get; set; }
        int CantidadAlumnosDesertores { get; set; }
        int HoraCatedra { get; set; }
        DateTime FechaAsistencia { get; set; }
        int PorcentajeAsistencia { get; set; }
        int CursoId { get; set; }
        string Materia { get; set; }
        int ModulosMateria { get; set; }
    }
}