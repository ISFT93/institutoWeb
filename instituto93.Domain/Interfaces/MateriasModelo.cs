using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    public class MateriasModelo
    {
        
        public int MateriaId { get; set; }
        
        
        public string Nombre { get; set; }
        public int AnioCarreraId { get; set; }
        public int EspacioId { get; set; }
        
      
        public int CargaHoraria { get; set; }
      
              public int Modulos { get; set; }
        public bool Activo { get; set; }


        // Propiedades Extras
        
        public int CarreraId { get; set; }

        
        public int AnioCarrera { get; set; }
    }
}
