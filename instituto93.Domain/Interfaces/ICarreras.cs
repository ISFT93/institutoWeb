using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Domain.Interfaces
{
    public interface ICarreras
    {
        int CarreraId { get; set; }
        string Nombre { get; set; }
        string Titulo { get; set; }
        string DescripcionCorta { get; set; }
        string JefeCatedra { get; set; }
        int AnioInicio { get; set; }
        int AnioFin { get; set; }
        bool Activo { get; set; }
        string PlanEstudio { get; set; }
        string Resolucion { get; set; }
        string Correlatividades { get; set; }
        string ImagenDescriptiva { get; set; }
        string NumeroExpediente { get; set; }
        int CantidadHoras { get; set; }
        int Duracion { get; set; }
        int CarreraEstadoId { get; set; }
        string CarrerasCodigoBloque { get; set; }
        bool PoseeMaterias { get; set; }
    }
}
