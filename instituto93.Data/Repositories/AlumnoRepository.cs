using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace instituto93.Data.Repositories
//lopez melany
{
    //lopez melany
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly Conexion _conexion;

        public AlumnoRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<AlumnoModelo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<AlumnoModelo>();
            const string sql = @"
                    SELECT
                        AlumnoId, Apellido, Nombre, TipoDocumento, NumeroDocumento, EstadoCivil, Sexo,
                        FechaNacimiento, LocalidadNacimiento, PaisNacimiento, Calle, Numero, Piso, Departamento,
                        Provincia, Distrito, Localidad, CodigoPostal, Telefono, Celular, Email, Activo
                    FROM Alumnos";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new AlumnoModelo
                    {
                        AlumnoId = reader.GetInt32(0),
                        Apellido = reader.GetString(1),
                        Nombre = reader.GetString(2),
                        TipoDocumento =  reader.GetString(3),
                        NumeroDocumento = reader.GetString(4),
                        EstadoCivil = reader.GetString(5),
                        Sexo = reader.GetString(6),
                        FechaNacimiento = reader.GetDateTime(7),
                        LocalidadNacimiento = reader.GetString(8),
                        PaisNacimiento = reader.GetString(9),
                        Calle = reader.GetString(10),
                        Numero = reader.GetString(11),
                        Piso = reader.GetString(12),
                        Departamento = reader.GetString(13),
                        Provincia = reader.GetString(14),
                        Distrito = reader.GetString(15),
                        Localidad = reader.GetString(16),
                        CodigoPostal = reader.GetString(17),
                        Telefono = reader.GetString(18),
                        Celular = reader.GetString(19),
                        Email = reader.GetString(20),
                        Activo = reader.GetBoolean(21)
                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<AlumnoModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                    SELECT
                        AlumnoId, Apellido, Nombre, TipoDocumento, NumeroDocumento, EstadoCivil, Sexo,
                        FechaNacimiento, LocalidadNacimiento, PaisNacimiento, Calle, Numero, Piso, Departamento,
                        Provincia, Distrito, Localidad, CodigoPostal, Telefono, Celular, Email,
                        TituloSecundario, MateriasAdeuda, DescripcionMaterias, Titulo, Orientacion, OtorgadoPor,
                        AnioEgreso, Promedio, TituloTramite, MayorTitulo, OtroTitulo, MayorOtorgadoPor, MayorPromedio,
                        FotocopiaTitulo, ConstanciaTituloTramite, ConstanciaAdeudaMaterias, CantidadAdeudaMaterias,
                        CertificadoAptitud, FotocopiaDocumento, FotoCarnet, FotocopiaPartidaNacimiento,
                        VacunaAntihepatitis, VacunaAntitetanica, Recibo, Monto, ObraSocialPrepaga, DescripcionObraSocial,
                        TratamientoMedico, DescripcionTratamiento, Medicacion, DescripcionMedicacion,
                        Discapacidad, DescripcionDiscapacidad, EstadoDiscapacidad, CertificadoDiscapacidad,
                        ContactoEmergencia, TelefonoContacto, Activo, FotoUrl, Carrera
                    FROM Alumnos
                    WHERE AlumnoId = @id";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new AlumnoModelo
                    {
                        AlumnoId = reader.GetInt32(0),
                        Apellido =  reader.GetString(1),
                        Nombre = reader.GetString(2),
                        TipoDocumento = reader.GetString(3),
                        NumeroDocumento =  reader.GetString(4),
                        EstadoCivil = reader.GetString(5),
                        Sexo = reader.GetString(6),
                        FechaNacimiento = reader.GetDateTime(7),
                        LocalidadNacimiento =  reader.GetString(8),
                        PaisNacimiento = reader.GetString(9),
                        Calle = reader.GetString(10),
                        Numero =  reader.GetString(11),
                        Piso = reader.GetString(12),
                        Departamento = reader.GetString(13),
                        Provincia = reader.GetString(14),
                        Distrito = reader.GetString(15),
                        Localidad = reader.GetString(16),
                        CodigoPostal = reader.GetString(17),
                        Telefono = reader.GetString(18),
                        Celular = reader.GetString(19),
                        Email = reader.GetString(20),
                        TituloSecundario = reader.GetBoolean(21),
                        MateriasAdeuda = reader.GetInt32(22),
                        DescripcionMaterias = reader.GetString(23),
                        Titulo = reader.GetString(24),
                        Orientacion = reader.GetString(25),
                        OtorgadoPor = reader.GetString(26),
                        AnioEgreso = reader.GetInt32(27),
                        Promedio = reader.GetDecimal(28),
                        TituloTramite = reader.GetBoolean(29),
                        MayorTitulo = reader.GetString(30),
                        OtroTitulo = reader.GetString(31),
                        MayorOtorgadoPor = reader.GetString(32),
                        MayorPromedio = reader.GetDecimal(33),
                        FotocopiaTitulo = reader.GetBoolean(34),
                        ConstanciaTituloTramite =  reader.GetBoolean(35),
                        ConstanciaAdeudaMaterias = reader.GetBoolean(36),
                        CantidadAdeudaMaterias = reader.GetInt32(37),
                        CertificadoAptitud = reader.GetBoolean(38),
                        FotocopiaDocumento = reader.GetBoolean(39),
                        FotoCarnet = reader.GetBoolean(40),
                        FotocopiaPartidaNacimiento = reader.GetBoolean(41),
                        VacunaAntihepatitis = reader.GetBoolean(42),
                        VacunaAntitetanica = reader.GetBoolean(43),
                        Recibo = reader.GetInt32(44),
                        Monto = reader.GetInt32(45),
                        ObraSocialPrepaga = reader.IsDBNull(46) ? (bool?)null : reader.GetBoolean(46),
                        DescripcionObraSocial = reader.GetString(47),
                        TratamientoMedico = reader.GetBoolean(48),
                        DescripcionTratamiento = reader.GetString(49),
                        Medicacion = reader.GetBoolean(50),
                        DescripcionMedicacion = reader.GetString(51),
                        Discapacidad = reader.GetBoolean(52),
                        DescripcionDiscapacidad = reader.GetString(53),
                        EstadoDiscapacidad = reader.GetString(54),
                        CertificadoDiscapacidad = reader.GetBoolean(55),
                        ContactoEmergencia = reader.GetString(56),
                        TelefonoContacto = reader.GetString(57),
                        Activo = reader.GetBoolean(58),
                        FotoUrl = reader.GetString(59),
                        Carrera = reader.GetString(60)
                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<int> CreateAsync(AlumnoModelo alumno, CancellationToken cancellationToken = default)
        {
            if (alumno == null) throw new ArgumentNullException(nameof(alumno));
            const string sql = @"
                    INSERT INTO Alumnos (
                        Apellido, Nombre, TipoDocumento, NumeroDocumento, FechaNacimiento, Activo, Carrera
                    ) VALUES (
                        @Apellido, @Nombre, @TipoDocumento, @NumeroDocumento, @FechaNacimiento, @Activo, @Carrera
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Apellido", (object?)alumno.Apellido ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nombre", (object?)alumno.Nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoDocumento", (object?)alumno.TipoDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NumeroDocumento", (object?)alumno.NumeroDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaNacimiento", (object?)alumno.FechaNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", (object?)alumno.Activo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Carrera", (object?)alumno.Carrera ?? DBNull.Value);

                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(AlumnoModelo alumno, CancellationToken cancellationToken = default)
        {
            if (alumno == null) throw new ArgumentNullException(nameof(alumno));
            const string sql = @"
                    UPDATE Alumnos SET
                        Apellido = @Apellido,
                        Nombre = @Nombre,
                        TipoDocumento = @TipoDocumento,
                        NumeroDocumento = @NumeroDocumento,
                        FechaNacimiento = @FechaNacimiento,
                        Activo = @Activo,
                        Carrera = @Carrera
                    WHERE AlumnoId = @id";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Apellido", (object?)alumno.Apellido ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nombre", (object?)alumno.Nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoDocumento", (object?)alumno.TipoDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NumeroDocumento", (object?)alumno.NumeroDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaNacimiento", (object?)alumno.FechaNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", (object?)alumno.Activo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Carrera", (object?)alumno.Carrera ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", alumno.AlumnoId);
                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM Alumnos WHERE AlumnoId = @id";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", id);
                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }
    }
}