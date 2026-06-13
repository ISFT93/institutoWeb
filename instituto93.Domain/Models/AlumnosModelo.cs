using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Domain.Models
{
    internal class AlumnosModelo
    {
            // Propiedades Propias de Alumnos

            [Clave]
            public int AlumnoId { get; set; }

            [Obligatorio("El apellido es obligatorio")]
            [SoloLetrasEspacios]
            public string Apellido { get; set; }
            [Obligatorio]
            [SoloLetrasEspacios]
            public string Nombre { get; set; }
            [Obligatorio]
            public string TipoDocumento { get; set; }
            [Obligatorio]
            [SoloNumeros]
            [Unico(Mensaje = "El documento ingresado ya existe")]
            public string NumeroDocumento { get; set; }
            [Obligatorio]
            public string EstadoCivil { get; set; }
            [Obligatorio]
            public char Sexo { get; set; }
            [Obligatorio]
            public DateTime FechaNacimiento { get; set; }
            [Obligatorio]
            [SoloLetrasEspacios]
            public string LocalidadNacimiento { get; set; }
            [SoloLetrasEspacios]
            [Obligatorio]
            public string PaisNacimiento { get; set; }
            [Obligatorio]
            [SoloLetrasNumerosEspacios]
            public string Calle { get; set; }
            [SoloNumeros]
            [Obligatorio]
            public string Numero { get; set; }

            [SoloNumeros]
            public string Piso { get; set; }

            [SoloLetrasNumerosEspacios]
            public string Departamento { get; set; }


            [Obligatorio]
            public string Provincia { get; set; }
            [SoloLetrasEspacios]
            [Obligatorio]
            public string Distrito { get; set; }
            [SoloLetrasEspacios]
            [Obligatorio]
            public string Localidad { get; set; }
            [SoloNumeros]
            [Obligatorio]
            public string CodigoPostal { get; set; }
            [SoloNumeros]
            [Obligatorio]
            public string Telefono { get; set; }
            [SoloNumeros]
            [Obligatorio]
            public string Celular { get; set; }
            [Email]
            [AlmenosUno("Telefono:Celular")]
            [Obligatorio]
            public string Email { get; set; }
            [Obligatorio]
            public bool TituloSecundario { get; set; }

            [Obligatorio(condicion: "TituloSecundarioInactivo")]
            [SoloNumeros(0, 30)]
            public int MateriasAdeuda { get; set; }
            [Obligatorio(condicion: "TituloSecundarioInactivo")]
            public string DescripcionMaterias { get; set; }
            [Obligatorio(condicion: "TituloSecundarioActivo")]
            [SoloLetrasEspacios]
            public string Titulo { get; set; }
            [Obligatorio(condicion: "TituloSecundarioActivo")]
            [SoloLetrasEspacios]
            public string Orientacion { get; set; }
            [Obligatorio(condicion: "TituloSecundarioActivo")]
            public string OtorgadoPor { get; set; }
            [SoloLetrasNumerosEspacios]
            [Obligatorio(condicion: "TituloSecundarioActivo")]
            [SoloNumeros]
            public int AnioEgreso { get; set; }
            [Obligatorio(condicion: "TituloSecundarioActivo")]
            [SoloNumeros(0, 10)]
            public decimal Promedio { get; set; }
            public bool TituloTramite { get; set; }
            [Obligatorio(condicion: "TituloSecundarioActivo")]
            public string MayorTitulo { get; set; }

            [Obligatorio(condicion: "MayorTituloAsignado")]
            [SoloLetrasEspacios]
            public string OtroTitulo { get; set; }

            [Obligatorio(condicion: "MayorTituloAsignado")]
            [SoloLetrasNumerosEspacios]
            public string MayorOtorgadoPor { get; set; }

            [Obligatorio(condicion: "MayorTituloAsignado")]
            [SoloNumeros(0, 10)]
            public decimal MayorPromedio { get; set; }
            [Obligatorio]
            public bool FotocopiaTitulo { get; set; }

            public bool ConstanciaTituloTramite { get; set; }

            public bool ConstanciaAdeudaMaterias { get; set; }

            [Obligatorio(condicion: "ConstanciaAdeudaMateriasActivo")]
            public int CantidadAdeudaMaterias { get; set; }

            public bool CertificadoAptitud { get; set; }

            public bool FotocopiaDocumento { get; set; }

            public bool FotoCarnet { get; set; }

            public bool FotocopiaPartidaNacimiento { get; set; }

            public bool VacunaAntihepatitis { get; set; }

            public bool VacunaAntitetanica { get; set; }
            [Obligatorio]
            [SoloNumeros]
            public int Recibo { get; set; }
            [Obligatorio]
            [SoloNumeros]
            public int Monto { get; set; }

            public bool ObraSocialPrepaga { get; set; }

            [Obligatorio(condicion: "ObraSocialPrepagaActiva")]
            public string DescripcionObraSocial { get; set; }

            public bool TratamientoMedico { get; set; }

            [Obligatorio(condicion: "TratamientoMedicoActiva")]
            public string DescripcionTratamiento { get; set; }

            public bool Medicacion { get; set; }
            [Obligatorio(condicion: "MedicacionActiva")]
            public string DescripcionMedicacion { get; set; }

            public bool Discapacidad { get; set; }
            [Obligatorio(condicion: "DiscapacidadActiva")]
            public string DescripcionDiscapacidad { get; set; }
            [Obligatorio(condicion: "DiscapacidadActiva")]
            public string EstadoDiscapacidad { get; set; }
            [Obligatorio(condicion: "DiscapacidadActiva")]
            public bool CertificadoDiscapacidad { get; set; }

            [Obligatorio]
            public string ContactoEmergencia { get; set; }

            [Obligatorio]
            [SoloNumeros]
            public string TelefonoContacto { get; set; }
            [Obligatorio]
            public string FotoUrl { get; set; }

            public bool Activo { get; set; }

            //Propiedades Extras

            [Ignorar]
            string Carrera { get; set; }

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
