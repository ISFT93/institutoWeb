using instituto93.Domain.Interfaces;

namespace instituto93.Domain.Models
{
    public class Localidad:ILocalidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }  

    }
}
