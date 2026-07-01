using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    public interface ICursoMaterias
    {
        string CursoMateriaId { get; set; }
        string MateriaId { get; set; }
        string CursoId { get; set; }
        string FechaAlta { get; set; }
        string FechaBaja { get; set; }
        string Activo { get; set; }

    }
}
