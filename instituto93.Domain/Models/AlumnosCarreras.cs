using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    // Acevedo Cecilia
    public class AlumnosCarreras
    {
        public int AlumnoCarreraId { get; set; }
        public int CarreraId { get; set; }
        public int AlumnoId { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public bool Activo { get; set; }
    }
}
