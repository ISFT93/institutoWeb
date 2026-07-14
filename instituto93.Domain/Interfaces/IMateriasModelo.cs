using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    public interface IMateriasModelo
    {
        //Romero Alejo
        int MateriaId { get; set; }

        string Nombre { get; set; }
        int AnioCarreraId { get; set; }
        int EspacioId { get; set; }


        int CargaHoraria { get; set; }

        int Modulos { get; set; }
        bool Activo { get; set; }

        // Propiedades Extras

        int CarreraId { get; set; }


        int AnioCarrera { get; set; }

    }
}
