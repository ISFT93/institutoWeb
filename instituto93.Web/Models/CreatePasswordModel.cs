using System.ComponentModel.DataAnnotations;

namespace instituto93.Web.Models;

public class CreatePasswordModel
{
    public const int MinPasswordLength = 8;

    public string Dni { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá una contraseña.")]
    [MinLength(MinPasswordLength, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmá la contraseña.")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
