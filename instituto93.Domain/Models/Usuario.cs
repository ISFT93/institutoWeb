using instituto93.Domain.Interfaces;

namespace instituto93.Domain.Models
{
    // Solo credenciales de acceso y rol. Los datos personales se obtienen de Alumno o Profesor.
    public class Usuario : IUsuario
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int? AlumnoId { get; set; }
        public int? ProfesorId { get; set; }
        public string Password { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public Rol? Rol { get; set; }
        public AlumnoModelo? Alumno { get; set; }
        public Profesores? Profesor { get; set; }

        public string NombreRol => Rol?.Nombre ?? string.Empty;
        public string NombreCompleto => (Profesor is not null
            ? $"{Profesor.Nombre} {Profesor.Apellido}"
            : $"{Alumno?.Nombre} {Alumno?.Apellido}").Trim();
        public string? Email => Profesor is not null ? Profesor.Email : Alumno?.Email;
        public bool PersonaActiva => Profesor is not null ? Profesor.Activo != 0 : Alumno?.Activo != false;
    }
}
