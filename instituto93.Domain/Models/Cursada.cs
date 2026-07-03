using System;

namespace instituto93.Domain.Models
{
    public class Cursada
    {
        
        public int CursadaId { get; set; }
        public int CursoMateriaId { get; set; }
        public required string AnioLectivo { get; set; }
        public required string Anio { get; set; }
        public int CantidadAlumnos { get; set; }
        public int CantidadAlumnosRecursantes { get; set; }
        public int CantidadAlumnosDesertores { get; set; }
        public int HoraCatedra { get; set; }
        public DateTime FechaAsistencia { get; set; }
        public int PorcentajeAsistencia { get; set; }
        public int CursoId { get; set; }
        public required string Materia { get; set; }
        public int ModulosMateria { get; set; }
    }
}