using instituto93.Domain.Interfaces;

namespace instituto93.Domain.Models
{
    // Solo credenciales de acceso. Los datos personales se obtienen de Alumno.
    public class Usuario : IUsuario
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public string Password { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public AlumnoModelo? Alumno { get; set; }
    }
}
