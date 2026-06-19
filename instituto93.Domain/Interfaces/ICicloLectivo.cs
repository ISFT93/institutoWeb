using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    public interface ICicloLectivo
    {
        public int AnioLectivo { get; set; }

        public int CantidadSemana { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaInscripcionInicio { get; set; }

        public DateTime? FechaInscripcionFinal { get; set; }

        public DateTime? FechaMarzoInicio { get; set; }

        public DateTime? FechaMarzoFinal { get; set; }

        public DateTime? FechaJunioInicio { get; set; }

        public DateTime? FechaJunioFinal { get; set; }

        public DateTime? FechaDiciembreInicio { get; set; }

        public DateTime? FechaDiciembreFinal { get; set; }

        public DateTime? FechaEspecialInicio { get; set; }

        public DateTime? FechaEspecialFinal { get; set; }

        public DateTime? FechaCierre { get; set; }

        public bool? Activo { get; set; }

        public DateTime? FechaPreInscripcionInicio { get; set; }

        public DateTime? FechaPreInscripcionFinal { get; set; }
    }
}
