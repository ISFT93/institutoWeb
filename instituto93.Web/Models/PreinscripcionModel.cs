using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace instituto93.Web.Models;

public class PreinscripcionModel
{
    // Carrera de interés
    [Required(ErrorMessage = "Debés seleccionar una carrera.")]
    public string Carrera { get; set; } = string.Empty;

    // Datos personales
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    public string TipoDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [RegularExpression(@"^[0-9]{7,8}$", ErrorMessage = "Ingresá un número de documento válido (7 u 8 dígitos).")]
    public string NumeroDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    public string Sexo { get; set; } = string.Empty;

    public string? EstadoCivil { get; set; }

    // Lugar de nacimiento
    [Required(ErrorMessage = "La localidad de nacimiento es obligatoria.")]
    [MaxLength(15, ErrorMessage = "La localidad de nacimiento no puede superar los 15 caracteres.")]
    public string LocalidadNacimiento { get; set; } = string.Empty;

    [Required(ErrorMessage = "El país de nacimiento es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El país de nacimiento no puede superar los 50 caracteres.")]
    public string PaisNacimiento { get; set; } = string.Empty;

    // Domicilio
    [Required(ErrorMessage = "La calle es obligatoria.")]
    [MaxLength(255, ErrorMessage = "La calle no puede superar los 255 caracteres.")]
    public string Calle { get; set; } = string.Empty;

    public string? Numero { get; set; }
    public string? Piso { get; set; }
    public string? Departamento { get; set; }

    [Required(ErrorMessage = "El distrito es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El distrito no puede superar los 50 caracteres.")]
    public string? Distrito { get; set; }

    [Required(ErrorMessage = "La provincia es obligatoria.")]
    public string? Provincia { get; set; }

    [Required(ErrorMessage = "La localidad es obligatoria.")]
    [MaxLength(50, ErrorMessage = "La localidad no puede superar los 50 caracteres.")]
    public string Localidad { get; set; } = string.Empty;

    public string? CodigoPostal { get; set; }

    // Contacto
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresá un correo electrónico válido.")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30, ErrorMessage = "El celular no puede superar los 30 caracteres.")]
    public string? Celular { get; set; }

    [MaxLength(30, ErrorMessage = "El teléfono no puede superar los 30 caracteres.")]
    public string? Telefono { get; set; }

    [MaxLength(100, ErrorMessage = "El contacto de emergencia no puede superar los 100 caracteres.")]
    public string? ContactoEmergencia { get; set; }

    [MaxLength(20, ErrorMessage = "El teléfono de contacto no puede superar los 20 caracteres.")]
    public string? TelefonoContacto { get; set; }

    // Estudios secundarios
    public bool TituloSecundario { get; set; }

    [MaxLength(50, ErrorMessage = "El título no puede superar los 50 caracteres.")]
    public string? Titulo { get; set; }

    [MaxLength(50, ErrorMessage = "La orientación no puede superar los 50 caracteres.")]
    public string? Orientacion { get; set; }

    [MaxLength(50, ErrorMessage = "La institución otorgante no puede superar los 50 caracteres.")]
    public string? OtorgadoPor { get; set; }

    [Range(1900, 2100, ErrorMessage = "Ingresá un año de egreso válido.")]
    public int? AnioEgreso { get; set; }

    [Range(0, 10, ErrorMessage = "El promedio debe estar entre 0 y 10.")]
    public decimal? Promedio { get; set; }

    public bool MateriasAdeuda { get; set; }

    [Range(0, 20, ErrorMessage = "La cantidad de materias adeudadas debe estar entre 0 y 20.")]
    public int? CantidadAdeudaMaterias { get; set; }

    [MaxLength(150, ErrorMessage = "La descripción de materias adeudadas no puede superar los 150 caracteres.")]
    public string? DescripcionMaterias { get; set; }

    public bool TituloTramite { get; set; }

    [MaxLength(50, ErrorMessage = "El mayor título no puede superar los 50 caracteres.")]
    public string? MayorTitulo { get; set; }

    [MaxLength(50, ErrorMessage = "El otro título no puede superar los 50 caracteres.")]
    public string? OtroTitulo { get; set; }

    [MaxLength(50, ErrorMessage = "La institución del mayor título no puede superar los 50 caracteres.")]
    public string? MayorOtorgadoPor { get; set; }

    [Range(0, 10, ErrorMessage = "El mayor promedio debe estar entre 0 y 10.")]
    public decimal? MayorPromedio { get; set; }

    // Documentación
    public IBrowserFile? FotocopiaTitulo { get; set; }
    public IBrowserFile? ConstanciaTituloTramite { get; set; }
    public IBrowserFile? ConstanciaAdeudaMaterias { get; set; }
    public IBrowserFile? CertificadoAptitud { get; set; }
    public IBrowserFile? FotocopiaDocumento { get; set; }
    public IBrowserFile? FotoCarnet { get; set; }
    public IBrowserFile? FotocopiaPartidaNacimiento { get; set; }
    public IBrowserFile? VacunaAntihepatitis { get; set; }
    public IBrowserFile? VacunaAntitetanica { get; set; }

    // Arancel
    [Range(1, int.MaxValue, ErrorMessage = "Ingresá un número de recibo válido.")]
    public int? Recibo { get; set; }
    
    // Salud
    public bool ObraSocialPrepaga { get; set; }

    [MaxLength(50, ErrorMessage = "La descripción de obra social no puede superar los 50 caracteres.")]
    public string? DescripcionObraSocial { get; set; }

    public bool TratamientoMedico { get; set; }

    [MaxLength(150, ErrorMessage = "La descripción del tratamiento no puede superar los 150 caracteres.")]
    public string? DescripcionTratamiento { get; set; }

    public bool Medicacion { get; set; }

    [MaxLength(150, ErrorMessage = "La descripción de la medicación no puede superar los 150 caracteres.")]
    public string? DescripcionMedicacion { get; set; }

    public bool Discapacidad { get; set; }

    [MaxLength(15, ErrorMessage = "El estado de discapacidad no puede superar los 15 caracteres.")]
    public string? EstadoDiscapacidad { get; set; }

    [MaxLength(150, ErrorMessage = "La descripción de discapacidad no puede superar los 150 caracteres.")]
    public string? DescripcionDiscapacidad { get; set; }

    public IBrowserFile? CertificadoDiscapacidad { get; set; }
}