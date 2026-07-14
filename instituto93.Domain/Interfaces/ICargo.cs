using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    public interface ICargo
    {
        int CargoId { get; set; }
        string Descripcion { get; set; }
        int TipoAsignacionId { get; set; }
        int TipoAplicacionId { get; set; }
        bool Activo { get; set; }
    }
}
