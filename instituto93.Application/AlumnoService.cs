using instituto93.Data.Repositories;
using instituto93.Domain.Interfaces;
using instituto93.Domain.Models;
using instituto93.Domain.DTOs;


namespace instituto93.Application
{
    //Looez Melany
    public class AlumnoService : IAlumnoService
    {
        private readonly IAlumnoRepository _repo;

        public AlumnoService(IAlumnoRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<AlumnoModelo>> GetAlumnosModelos(
            CancellationToken cancellationToken = default)
        {
            var alumnos = await _repo.GetAllAsync(cancellationToken);

            return alumnos.ToList();
        }

        public Task<AlumnoModelo?> GetAlumnoByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return _repo.GetByIdAsync(id, cancellationToken);
        }

        public Task<int> CreatePreinscripcionAsync(
            PreinscripcionDto p,
            CancellationToken cancellationToken = default)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            if (!p.FechaNacimiento.HasValue)
                throw new ArgumentException(
                    "La fecha de nacimiento es obligatoria.");

            var alumno = new AlumnoModelo
            {
                Apellido = p.Apellido,
                Nombre = p.Nombre,
                TipoDocumento = p.TipoDocumento,
                NumeroDocumento = p.NumeroDocumento,

                EstadoCivil = p.EstadoCivil,
                Sexo = p.Sexo,

                FechaNacimiento = p.FechaNacimiento.Value,

                LocalidadNacimiento = p.LocalidadNacimiento,
                PaisNacimiento = p.PaisNacimiento,

                Calle = p.Calle,
                Numero = p.Numero,
                Piso = p.Piso,
                Departamento = p.Departamento,

                Provincia = p.Provincia,
                Distrito = p.Distrito,
                Localidad = p.Localidad,
                CodigoPostal = p.CodigoPostal,

                Telefono = p.Telefono,
                Celular = p.Celular,
                Email = p.Email,

                TituloSecundario = p.TituloSecundario,

                MateriasAdeuda =
                    p.CantidadAdeudaMaterias > 0 ? 1 : 0,

                DescripcionMaterias =
                    p.DescripcionMaterias,

                Titulo = p.Titulo,
                Orientacion = p.Orientacion,
                OtorgadoPor = p.OtorgadoPor,

                AnioEgreso = p.AnioEgreso,
                Promedio = p.Promedio,

                TituloTramite = p.TituloTramite,

                MayorTitulo = p.MayorTitulo,
                OtroTitulo = p.OtroTitulo,

                MayorOtorgadoPor =
                    p.MayorOtorgadoPor,

                MayorPromedio =
                    p.MayorPromedio,

                FotocopiaTitulo =
                    p.FotocopiaTitulo,

                ConstanciaTituloTramite =
                    p.ConstanciaTituloTramite,

                ConstanciaAdeudaMaterias =
                    p.ConstanciaAdeudaMaterias,

                CantidadAdeudaMaterias =
                    p.CantidadAdeudaMaterias,

                CertificadoAptitud =
                    p.CertificadoAptitud,

                FotocopiaDocumento =
                    p.FotocopiaDocumento,

                FotoCarnet =
                    p.FotoCarnet,

                FotocopiaPartidaNacimiento =
                    p.FotocopiaPartidaNacimiento,

                VacunaAntihepatitis =
                    p.VacunaAntihepatitis,

                VacunaAntitetanica =
                    p.VacunaAntitetanica,

                Recibo = p.Recibo,

                Monto = p.Monto,

                ObraSocialPrepaga =
                    p.ObraSocialPrepaga,

                DescripcionObraSocial =
                    p.DescripcionObraSocial,

                TratamientoMedico =
                    p.TratamientoMedico,

                DescripcionTratamiento =
                    p.DescripcionTratamiento,

                Medicacion =
                    p.Medicacion,

                DescripcionMedicacion =
                    p.DescripcionMedicacion,

                Discapacidad =
                    p.Discapacidad,

                DescripcionDiscapacidad =
                    p.DescripcionDiscapacidad,

                EstadoDiscapacidad =
                    p.EstadoDiscapacidad,

                CertificadoDiscapacidad =
                    p.CertificadoDiscapacidad,

                ContactoEmergencia =
                    p.ContactoEmergencia,

                TelefonoContacto =
                    p.TelefonoContacto,

                Activo = true,

                FotoUrl = null,

                Carrera = p.Carrera
            };

            return _repo.CreateAsync(
                alumno,
                cancellationToken);
        }
    }


}
