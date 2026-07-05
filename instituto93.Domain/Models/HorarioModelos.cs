using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    //Mourdad Ignacio
    public class HorariosModelo

    {
        public int HorarioId { get; set; }
        public int? DiaId { get; set; }
        public int? ModuloId { get; set; }
        public int CursoMateriaId { get; set; }
        
        public int MateriaId { get; set; }
        
        public int CursoId { get; set; }
        
        public string Nombre { get; set; }
        
    
    }
}
