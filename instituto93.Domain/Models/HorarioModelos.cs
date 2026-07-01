using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    //Mourdad Ignacio
    public class HorariosModelo : ModeloBase
    {
        [Clave]
        public int HorarioId { get; set; }
        public int? DiaId { get; set; }
        public int? ModuloId { get; set; }
        public int CursoMateriaId { get; set; }
        [Ignorar]
        public int MateriaId { get; set; }
        [Ignorar]
        public int CursoId { get; set; }
        [Ignorar]
        public string Nombre { get; set; }
        [Ignorar]
        public bool Asignado { get => ModuloId != null && DiaId != null; }
    }
}
