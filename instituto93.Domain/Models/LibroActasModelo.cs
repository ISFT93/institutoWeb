using instituto93.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    public class LibroActasModelo : ILibroActas
    {
        public int LibroActaId { get; set; }
        public int LibroNumero { get; set; }
        public int FolioNumero { get; set; }
        public int FolioMaximo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public bool Activo { get; set; }
        public int TipoLibroId { get; set; }
    }
}
