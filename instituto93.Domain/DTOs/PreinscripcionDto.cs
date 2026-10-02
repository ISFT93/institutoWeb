using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.DTOs
{
    public class PreinscripcionDto
    {
        [Required]
        public string Carrera { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string TipoDocumento { get; set; } = string.Empty;

        [Required]
        public string NumeroDocumento { get; set; } = string.Empty;

        public string? EstadoCivil { get; set; }

        [Required]
        public string Sexo { get; set; } = string.Empty;

        [Required]
        public DateTime? FechaNacimiento { get; set; }

        [Required]
        public string LocalidadNacimiento { get; set; } = string.Empty;

        [Required]
        public string PaisNacimiento { get; set; } = string.Empty;

        [Required]
        public string Calle { get; set; } = string.Empty;

        public string? Numero { get; set; }

        public string? Piso { get; set; }

        public string? Departamento { get; set; }

        public string? Provincia { get; set; }

        public string? Distrito { get; set; }

        [Required]
        public string Localidad { get; set; } = string.Empty;

        public string? CodigoPostal { get; set; }

        public string? Telefono { get; set; }

        public string? Celular { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool TituloSecundario { get; set; }

        public string? DescripcionMaterias { get; set; }

        public string? Titulo { get; set; }

        public string? Orientacion { get; set; }

        public string? OtorgadoPor { get; set; }

        public int? AnioEgreso { get; set; }

        public decimal? Promedio { get; set; }

        public bool TituloTramite { get; set; }

        public string? MayorTitulo { get; set; }

        public string? OtroTitulo { get; set; }

        public string? MayorOtorgadoPor { get; set; }

        public decimal? MayorPromedio { get; set; }

        public bool FotocopiaTitulo { get; set; }

        public bool ConstanciaTituloTramite { get; set; }

        public bool ConstanciaAdeudaMaterias { get; set; }

        public int? CantidadAdeudaMaterias { get; set; }

        public bool CertificadoAptitud { get; set; }

        public bool FotocopiaDocumento { get; set; }

        public bool FotoCarnet { get; set; }

        public bool FotocopiaPartidaNacimiento { get; set; }

        public bool VacunaAntihepatitis { get; set; }

        public bool VacunaAntitetanica { get; set; }

        public int? Recibo { get; set; }

        public int? Monto { get; set; }

        public bool ObraSocialPrepaga { get; set; }

        public string? DescripcionObraSocial { get; set; }

        public bool TratamientoMedico { get; set; }

        public string? DescripcionTratamiento { get; set; }

        public bool Medicacion { get; set; }

        public string? DescripcionMedicacion { get; set; }

        public bool Discapacidad { get; set; }

        public string? DescripcionDiscapacidad { get; set; }

        public string? EstadoDiscapacidad { get; set; }

        public bool CertificadoDiscapacidad { get; set; }

        public string? ContactoEmergencia { get; set; }

        public string? TelefonoContacto { get; set; }
    }
}
