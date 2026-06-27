using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    public interface IInscripcionMateria
    {
        int cursadaAlumnoId { get; set; }
        string estado { get; set; }
        string cursada { get; set; }
    }
}
