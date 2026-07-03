using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instituto93.Domain.Interfaces;

namespace instituto93.Domain.Models
{
    public class InscripcionMateria : IInscripcionMateria
    {
        //Fernandez Franco
        public int cursadaAlumnoId { get; set; }
        public string estado { get; set; }
        public string cursada { get; set; }
    }
}
