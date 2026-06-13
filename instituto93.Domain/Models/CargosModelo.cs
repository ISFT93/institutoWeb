using instituto93.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    //Alumno : Ruiz Besson Dilan
    public class CargosModelo : ICargos
    {
        public int CargoId { get; set; }
        public string Descripcion { get; set; }
        public int TipoAsignacionId { get; set; }
        public int TipoAplicacionId { get; set; }
        public bool Activo { get; set; }
    }
}
