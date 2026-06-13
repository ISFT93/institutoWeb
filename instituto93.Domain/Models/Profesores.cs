using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    public class Profesores
    {
        public int ProfesorId { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string FechaNacimiento { get; set; }
        public char Sexo { get; set; }
        public string Direccion { get; set; }
        public string Piso { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public string Nacionalidad { get; set; }
        public string Email { get; set; }
        public string EstadoCivil { get; set; }
        public string Foto { get; set; }
        public byte Activo { get; set; }

        public string Titulo { get; set; }
        public byte TramoPedagogico { get; set; }
        public int ProfesorEstadoId { get; set; }

    }
}
