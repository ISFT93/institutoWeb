using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    public interface IPersona
    {
    int Id { get; set; }
    string Nombre { get; set; }
    string Apellido { get; set; }
    DateTime FechaNacimiento { get; set; }
    string Dni { get; set; }
    string Telefono { get; set; }
    string Direccion { get; set; }
    Localidad Localidad { get; set; }

}
}
