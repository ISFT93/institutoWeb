using System.ComponentModel.DataAnnotations;

namespace instituto93.Domain.DTOs
{
    public class AlumnoPruebaDto
    {
        public int Dni { get; set; }

        [StringLength(30, ErrorMessage = "El campo debe tener 30 o menos caracteres.")]
        public string Nombre { get; set; }

        [StringLength(30, ErrorMessage = "El campo debe tener 30 o menos caracteres.")]
        public string Apellido { get; set; }

        [StringLength(2, ErrorMessage = "El campo no debe tener más de 2 caracteres.")]
        public string Libro { get; set; }
        public int Folio { get; set; }
        public int Ingreso { get; set; }

        [StringLength(30, ErrorMessage = "El campo debe tener 20 o menos caracteres.")]
        public string Nacionalidad { get; set; }
    }
}
