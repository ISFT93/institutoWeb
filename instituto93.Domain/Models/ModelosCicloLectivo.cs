using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    internal class ModelosCicloLectivo
    {
        //Datos INSERT CicloLectivo
        public int ciclolectivo { get; set; }
        public int txtCantidadSemanas { get; set; }
        public DateTime fechaInicio { set { this.fechaInicioStr = value.ToString("yyyy-MM-dd"); } }
        public DateTime fechaFinal { set { this.fechaFinalStr = value.ToString("yyyy-MM-dd"); } }
        public DateTime fechaInscInicio { set { this.fechaInscInicioStr = value.ToString("yyyy-MM-dd"); } }
        public DateTime fechaInscFinal { set { this.fechaInscFinalStr = value.ToString("yyyy-MM-dd"); } }

        //Mascara fechas INSERT CicloLectivo
        public string fechaInicioStr { get; set; }
        public string fechaFinalStr { get; set; }
        public string fechaInscInicioStr { get; set; }
        public string fechaInscFinalStr { get; set; }

        //Datos UPDATE CicloLectivos
        public DateTime iniMarzo { set { this.iniMarzoStr = value.ToString("yyyy-MM-dd"); } }
        public DateTime finMarzo { set { this.finMarzoStr = value.ToString("yyyy-MM-dd"); } }
        public DateTime iniJunio { set { this.iniJunioStr = value.ToString("yyyy-MM-dd"); } }
        public DateTime finJunio { set { this.finJunioStr = value.ToString("yyyy-MM-dd"); } }
        public DateTime iniDiciembre { set { this.iniDiciembreStr = value.ToString("yyyy-MM-dd"); } }
        public DateTime finDiciembre { set { this.finDiciembreStr = value.ToString("yyyy-MM-dd"); } }
        public DateTime iniEspecial { set { this.iniEspecialStr = value.ToString("yyyy-MM-dd"); } }
        public DateTime finEspecial { set { this.finEspecialStr = value.ToString("yyyy-MM-dd"); } }

        //Mascara fechas UPDATE CicloLectivos
        public string iniMarzoStr { get; set; }
        public string finMarzoStr { get; set; }
        public string iniJunioStr { get; set; }
        public string finJunioStr { get; set; }
        public string iniDiciembreStr { get; set; }
        public string finDiciembreStr { get; set; }
        public string iniEspecialStr { get; set; }
        public string finEspecialStr { get; set; }

        //SP Cargar Cursadas Datos
        public int CursoMateriaId { get; set; }
        public int AnioLectivoId { get; set; }
        public int Anio { get; set; }
    }
}
