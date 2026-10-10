namespace instituto93.Domain.Models
{
    public class Rol
    {
        public const string Alumno = "Alumno";
        public const string Docente = "Docente";

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
