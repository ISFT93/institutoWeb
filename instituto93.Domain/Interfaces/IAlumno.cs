using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Interfaces
{
    //Lopez Melany
    public interface IAlumno
    {
        int AlumnoId { get; set; }

        string Apellido { get; set; }
        string Nombre { get; set; }
        string TipoDocumento { get; set; }
        string NumeroDocumento { get; set; }

        string? EstadoCivil { get; set; }
        string Sexo { get; set; }

        DateTime FechaNacimiento { get; set; }
        string LocalidadNacimiento { get; set; }
        string PaisNacimiento { get; set; }
        string Calle { get; set; }
        string? Numero { get; set; }
        string? Piso { get; set; }
        string? Departamento { get; set; }

        string? Provincia { get; set; }
        string? Distrito { get; set; }
        string Localidad { get; set; }
        string? CodigoPostal { get; set; }

        string? Telefono { get; set; }
        string? Celular { get; set; }
        string? Email { get; set; }

        bool? TituloSecundario { get; set; }
        int? MateriasAdeuda { get; set; }
        string? DescripcionMaterias { get; set; }

        string? Titulo { get; set; }
        string? Orientacion { get; set; }
        string? OtorgadoPor { get; set; }
        int? AnioEgreso { get; set; }
        decimal? Promedio { get; set; }

        bool? TituloTramite { get; set; }

        string? MayorTitulo { get; set; }
        string? OtroTitulo { get; set; }
        string? MayorOtorgadoPor { get; set; }
        decimal? MayorPromedio { get; set; }

        bool? FotocopiaTitulo { get; set; }
        bool? ConstanciaTituloTramite { get; set; }
        bool? ConstanciaAdeudaMaterias { get; set; }
        int? CantidadAdeudaMaterias { get; set; }

        bool? CertificadoAptitud { get; set; }
        bool? FotocopiaDocumento { get; set; }
        bool? FotoCarnet { get; set; }
        bool? FotocopiaPartidaNacimiento { get; set; }

        string? DescripcionObraSocial { get; set; }

        bool? TratamientoMedico { get; set; }
        string? DescripcionTratamiento { get; set; }

        bool? Medicacion { get; set; }
        string? DescripcionMedicacion { get; set; }

        bool? Discapacidad { get; set; }
        string? DescripcionDiscapacidad { get; set; }
        string? EstadoDiscapacidad { get; set; }
        bool? CertificadoDiscapacidad { get; set; }

        string? ContactoEmergencia { get; set; }
        string? TelefonoContacto { get; set; }

        bool? Activo { get; set; }
        string? FotoUrl { get; set; }
    }
}