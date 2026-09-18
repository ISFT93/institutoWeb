using System.ComponentModel.DataAnnotations;

namespace instituto93.Web.Models;

public class LoginModel
{
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    public string EmailOrDni { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;
}
