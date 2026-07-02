using instituto93.Domain.Interfaces;

namespace instituto93.Domain.Models
{
    public class Parametro:IParametro
    {
        public int ParametroId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set; }
        public Int16 TipoId { get; set; }
        public bool Activo { get; set; }
    }
}