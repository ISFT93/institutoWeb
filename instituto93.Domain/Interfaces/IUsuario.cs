namespace instituto93.Domain.Interfaces
{
    public interface IUsuario
    {
        int AlumnoId { get; set; }
        string Password { get; set; }
        bool Activo { get; set; }
    }
}
