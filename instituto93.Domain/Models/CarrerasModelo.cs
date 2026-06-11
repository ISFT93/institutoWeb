using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    public class CarrerasModelo
    {
        public int CarreraId { get; set; }
        public string Nombre { get; set; }
        public string Titulo { get; set; }
        public string DescripcionCorta { get; set; }
        public string JefeCatedra { get; set; }
        public int AnioInicio { get; set; }
        public int AnioFin { get; set; }
        public bool Activo { get; set; }
        public string PlanEstudio { get; set; }
        public string Resolucion { get; set; }
        public string Correlatividades { get; set; }
        public string ImagenDescriptiva { get; set; }
        public string NumeroExpediente { get; set; }
        public int CantidadHoras { get; set; }
        public int Duracion { get; set; }
        public int CarreraEstadoId { get; set; }
        public bool PoseeMaterias { get; set; }
    }
}
