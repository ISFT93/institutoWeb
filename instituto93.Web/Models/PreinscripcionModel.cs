using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace instituto93.Web.Models;

public class PreinscripcionModel
{
    //CARRERA DE INTERÉS
    [Required(ErrorMessage = "Debés seleccionar una carrera.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debés seleccionar una carrera.")]
    public int? CarreraId { get; set; }

    //DATOS PERSONALES
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    public string TipoDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Ingresá un número de documento válido (8 dígitos).")]
    public string NumeroDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    public string Sexo { get; set; } = string.Empty;



    //DOMICILIO
    [Required(ErrorMessage = "La calle es obligatoria.")]
    [MaxLength(255, ErrorMessage = "La calle no puede superar los 255 caracteres.")]
    public string Calle { get; set; } = string.Empty;

    [Required(ErrorMessage = "La localidad es obligatoria.")]
    [MaxLength(50, ErrorMessage = "La localidad no puede superar los 50 caracteres.")]
    public string Localidad { get; set; } = string.Empty;

    //CONTACTO
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresá un correo electrónico válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [MaxLength(30, ErrorMessage = "El teléfono no puede superar los 30 caracteres.")]
    public string? Telefono { get; set; }

}