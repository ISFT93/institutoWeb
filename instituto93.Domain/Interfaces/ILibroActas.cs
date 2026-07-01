using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    public interface ILibroActas
    {
        int LibroActaId { get; set; }
        int LibroNumero { get; set; }
        int FolioNumero { get; set; }
        int FolioMaximo { get; set; }
        DateTime FechaAlta { get; set; }
        DateTime? FechaBaja { get; set; }
        bool Activo { get; set; }
        int TipoLibroId { get; set; }
    }
}
