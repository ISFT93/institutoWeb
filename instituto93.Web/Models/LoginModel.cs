using System.ComponentModel.DataAnnotations;

namespace instituto93.Web.Models;

public class LoginModel
{
    [Required(ErrorMessage = "El D.N.I. es obligatorio.")]
    public string Dni { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;
}
