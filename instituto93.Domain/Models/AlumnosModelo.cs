using System;

namespace instituto93.Domain.Models
{
    //Lopez Melany
    public class AlumnosModelo
    {
        // Propiedades Propias de Alumnos
        public int AlumnoId { get; set; }
        public string? Apellido { get; set; }
        public string? Nombre { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? EstadoCivil { get; set; }
        public char? Sexo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? LocalidadNacimiento { get; set; }
        public string? PaisNacimiento { get; set; }
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? Piso { get; set; }
        public string? Departamento { get; set; }
        public string? Provincia { get; set; }
        public string? Distrito { get; set; }
        public string? Localidad { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public string? Email { get; set; }
        public bool? TituloSecundario { get; set; }
        public int? MateriasAdeuda { get; set; }
        public string? DescripcionMaterias { get; set; }
        public string? Titulo { get; set; }
        public string? Orientacion { get; set; }
        public string? OtorgadoPor { get; set; }
        public int? AnioEgreso { get; set; }
        public decimal? Promedio { get; set; }
        public bool? TituloTramite { get; set; }
        public string? MayorTitulo { get; set; }
        public string? OtroTitulo { get; set; }
        public string? MayorOtorgadoPor { get; set; }
        public decimal? MayorPromedio { get; set; }
        public bool? FotocopiaTitulo { get; set; }
        public bool? ConstanciaTituloTramite { get; set; }
        public bool? ConstanciaAdeudaMaterias { get; set; }
        public int? CantidadAdeudaMaterias { get; set; }
        public bool? CertificadoAptitud { get; set; }
        public bool? FotocopiaDocumento { get; set; }
        public bool? FotoCarnet { get; set; }
        public bool? FotocopiaPartidaNacimiento { get; set; }
        public bool? VacunaAntihepatitis { get; set; }
        public bool? VacunaAntitetanica { get; set; }
        public int? Recibo { get; set; }
        public int? Monto { get; set; }
        public bool? ObraSocialPrepaga { get; set; }
        public string? DescripcionObraSocial { get; set; }
        public bool? TratamientoMedico { get; set; }
        public string? DescripcionTratamiento { get; set; }
        public bool? Medicacion { get; set; }
        public string? DescripcionMedicacion { get; set; }
        public bool? Discapacidad { get; set; }
        public string? DescripcionDiscapacidad { get; set; }
        public string? EstadoDiscapacidad { get; set; }
        public bool? CertificadoDiscapacidad { get; set; }
        public string? ContactoEmergencia { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? FotoUrl { get; set; }
        public bool? Activo { get; set; }

        // Propiedades Extras
        public string? Carrera { get; set; }

        public bool TituloSecundarioActivo() => TituloSecundario == true;
        public bool TituloSecundarioInactivo() => TituloSecundario == false;
        public bool MayorTituloAsignado() => !string.IsNullOrEmpty(MayorTitulo) && MayorTitulo != "Ninguno";
        public bool ConstanciaAdeudaMateriasActivo() => ConstanciaAdeudaMaterias == true;
        public bool ObraSocialPrepagaActiva() => ObraSocialPrepaga == true;
        public bool TratamientoMedicoActiva() => TratamientoMedico == true;
        public bool MedicacionActiva() => Medicacion == true;
        public bool DiscapacidadActiva() => Discapacidad == true;
        public bool CertificadoDiscapacidadActiva() => CertificadoDiscapacidad == true;
    }
}