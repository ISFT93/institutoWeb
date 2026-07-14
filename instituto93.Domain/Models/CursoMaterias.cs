using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instituto93.Domain.Interfaces;    

namespace instituto93.Domain.Models
{
    public class CursoMaterias:ICursoMaterias
    {
        public string CursoMateriaId { get; set; }
        public string MateriaId { get; set; }
        public string CursoId { get; set; }
        public string FechaAlta { get; set; }
        public string FechaBaja { get; set; }
        public string Activo { get; set; }

    }
}
