using System.ComponentModel.DataAnnotations;

namespace instituto93.Domain.DTOs
{
    public class AltaPDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una carrera.")]
        public int? CarreraId { get; set; }

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string TipoDocumento { get; set; } = string.Empty;

        [Required]
        public string NumeroDocumento { get; set; } = string.Empty;

        [Required]
        public string Sexo { get; set; } = string.Empty;

        [Required]
        public DateTime? FechaNacimiento { get; set; }

        [Required]
        public string Calle { get; set; } = string.Empty;

        [Required]
        public string Localidad { get; set; } = string.Empty;

        public string? Telefono { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}
