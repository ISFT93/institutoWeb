using instituto93.Domain.Interfaces;

namespace instituto93.Domain.Models
{
    public class Pais : IPais
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
    }
}
