using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    public class CursoModelo
    {
        public int CursoId { get; set; }
        public string NombreCurso { get; set; }
        public int CicloLectivoId { get; set; }
        public int AnioCarreraId { get; set; }
        public bool Activo { get; set; }
        public bool AdmiteCurso { get; set; }
         public int AnioCarrera { get; set; }

    }
}
