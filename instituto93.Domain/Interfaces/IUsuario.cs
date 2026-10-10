namespace instituto93.Domain.Interfaces
{
    public interface IUsuario
    {
        int RolId { get; set; }
        int? AlumnoId { get; set; }
        int? ProfesorId { get; set; }
        string Password { get; set; }
        bool Activo { get; set; }
    }
}
