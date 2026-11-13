using System.ComponentModel.DataAnnotations;

namespace instituto93.Web.Models
{
    public class AlumnoPruebaModel
    {
        public int? Dni { get; set; }

        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public string? Libro { get; set; }
        public int? Folio { get; set; }
        public int? Ingreso { get; set; }

        public string? Nacionalidad { get; set; }
    }
}
