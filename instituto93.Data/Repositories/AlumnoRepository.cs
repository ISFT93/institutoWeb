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

        // Quita espacios, puntos y guiones de NumeroDocumento, igual que la normalización del DNI ingresado.
        public static string NumeroDocumentoNormalizadoSql(string alias) =>
            $"REPLACE(REPLACE(REPLACE(LTRIM(RTRIM({alias}.NumeroDocumento)), '.', ''), ' ', ''), '-', '')";

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

        public async Task<AlumnoModelo?> GetByDocumentoAsync(string numeroDocumento, CancellationToken cancellationToken = default)
        {
            var sql = $@"
                    SELECT TOP 1
                        AlumnoId, Apellido, Nombre, NumeroDocumento, FechaNacimiento,
                        Email, Telefono, Celular, Calle, Numero, Localidad, Activo
                    FROM Alumnos a
                    WHERE {NumeroDocumentoNormalizadoSql("a")} = @numeroDocumento
                    ORDER BY AlumnoId";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@numeroDocumento", System.Data.SqlDbType.VarChar, 30).Value = numeroDocumento;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (!await reader.ReadAsync(cancellationToken))
                    return null;

                string Text(int ordinal) => reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);

                return new AlumnoModelo
                {
                    AlumnoId = reader.GetInt32(0),
                    Apellido = Text(1),
                    Nombre = Text(2),
                    NumeroDocumento = Text(3),
                    FechaNacimiento = reader.IsDBNull(4) ? default : reader.GetDateTime(4),
                    Email = Text(5),
                    Telefono = Text(6),
                    Celular = Text(7),
                    Calle = Text(8),
                    Numero = Text(9),
                    Localidad = Text(10),
                    Activo = reader.IsDBNull(11) ? null : reader.GetBoolean(11)
                };
            }
            finally
            {
                _conexion.Close();
            }
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
            const string sql = @"
            INSERT INTO Alumnos (
            Apellido,
            Nombre,
            TipoDocumento,
            NumeroDocumento,
            EstadoCivil,
            Sexo,
            FechaNacimiento,
            LocalidadNacimiento,
            PaisNacimiento,
            Calle,
            Numero,
            Piso,
            Departamento,
            Provincia,
            Distrito,
            Localidad,
            CodigoPostal,
            Telefono,
            Celular,
            Email,
            TituloSecundario,
            MateriasAdeuda,
            DescripcionMaterias,
            Titulo,
            Orientacion,
            OtorgadoPor,
            AnioEgreso,
            Promedio,
            TituloTramite,
            MayorTitulo,
            OtroTitulo,
            MayorOtorgadoPor,
            MayorPromedio,
            FotocopiaTitulo,
            ConstanciaTituloTramite,
            ConstanciaAdeudaMaterias,
            CantidadAdeudaMaterias,
            CertificadoAptitud,
            FotocopiaDocumento,
            FotoCarnet,
            FotocopiaPartidaNacimiento,
            VacunaAntihepatitis,
            VacunaAntitetanica,
            Recibo,
            Monto,
            ObraSocialPrepaga,
            DescripcionObraSocial,
            TratamientoMedico,
            DescripcionTratamiento,
            Medicacion,
            DescripcionMedicacion,
            Discapacidad,
            DescripcionDiscapacidad,
        EstadoDiscapacidad,
        CertificadoDiscapacidad,
        ContactoEmergencia,
        TelefonoContacto,
        FotoUrl,
        Activo
        )
        VALUES (
        @Apellido,
        @Nombre,
        @TipoDocumento,
        @NumeroDocumento,
        @EstadoCivil,
        @Sexo,
        @FechaNacimiento,
        @LocalidadNacimiento,
        @PaisNacimiento,
        @Calle,
        @Numero,
        @Piso,
        @Departamento,
        @Provincia,
        @Distrito,
        @Localidad,
        @CodigoPostal,
        @Telefono,
        @Celular,
        @Email,
        @TituloSecundario,
        @MateriasAdeuda,
        @DescripcionMaterias,
        @Titulo,
        @Orientacion,
        @OtorgadoPor,
        @AnioEgreso,
        @Promedio,
        @TituloTramite,
        @MayorTitulo,
        @OtroTitulo,
        @MayorOtorgadoPor,
        @MayorPromedio,
        @FotocopiaTitulo,
        @ConstanciaTituloTramite,
        @ConstanciaAdeudaMaterias,
        @CantidadAdeudaMaterias,
        @CertificadoAptitud,
        @FotocopiaDocumento,
        @FotoCarnet,
        @FotocopiaPartidaNacimiento,
        @VacunaAntihepatitis,
        @VacunaAntitetanica,
        @Recibo,
        @Monto,
        @ObraSocialPrepaga,
        @DescripcionObraSocial,
        @TratamientoMedico,
        @DescripcionTratamiento,
        @Medicacion,
        @DescripcionMedicacion,
        @Discapacidad,
        @DescripcionDiscapacidad,
        @EstadoDiscapacidad,
        @CertificadoDiscapacidad,
        @ContactoEmergencia,
        @TelefonoContacto,
        @FotoUrl,
        @Activo
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
                cmd.Parameters.AddWithValue("@EstadoCivil", (object?)alumno.EstadoCivil ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Sexo", (object?)alumno.Sexo ?? DBNull.Value);
                cmd.Parameters.AddWithValue(
                    "@FechaNacimiento",
                    alumno.FechaNacimiento == default ? DBNull.Value : alumno.FechaNacimiento
                );
                cmd.Parameters.AddWithValue("@LocalidadNacimiento", (object?)alumno.LocalidadNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PaisNacimiento", (object?)alumno.PaisNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Calle", (object?)alumno.Calle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Numero", (object?)alumno.Numero ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Piso", (object?)alumno.Piso ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Departamento", (object?)alumno.Departamento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Provincia", (object?)alumno.Provincia ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Distrito", (object?)alumno.Distrito ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Localidad", (object?)alumno.Localidad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CodigoPostal", (object?)alumno.CodigoPostal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Telefono", (object?)alumno.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Celular", (object?)alumno.Celular ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object?)alumno.Email ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@TituloSecundario", (object?)alumno.TituloSecundario ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MateriasAdeuda", (object?)alumno.MateriasAdeuda ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DescripcionMaterias", (object?)alumno.DescripcionMaterias ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Titulo", (object?)alumno.Titulo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Orientacion", (object?)alumno.Orientacion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OtorgadoPor", (object?)alumno.OtorgadoPor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AnioEgreso", (object?)alumno.AnioEgreso ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Promedio", (object?)alumno.Promedio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TituloTramite", (object?)alumno.TituloTramite ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@MayorTitulo", (object?)alumno.MayorTitulo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OtroTitulo", (object?)alumno.OtroTitulo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MayorOtorgadoPor", (object?)alumno.MayorOtorgadoPor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MayorPromedio", (object?)alumno.MayorPromedio ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@FotocopiaTitulo", (object?)alumno.FotocopiaTitulo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ConstanciaTituloTramite", (object?)alumno.ConstanciaTituloTramite ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ConstanciaAdeudaMaterias", (object?)alumno.ConstanciaAdeudaMaterias ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CantidadAdeudaMaterias", (object?)alumno.CantidadAdeudaMaterias ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CertificadoAptitud", (object?)alumno.CertificadoAptitud ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FotocopiaDocumento", (object?)alumno.FotocopiaDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FotoCarnet", (object?)alumno.FotoCarnet ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FotocopiaPartidaNacimiento", (object?)alumno.FotocopiaPartidaNacimiento ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@VacunaAntihepatitis", (object?)alumno.VacunaAntihepatitis ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@VacunaAntitetanica", (object?)alumno.VacunaAntitetanica ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Recibo", (object?)alumno.Recibo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Monto", (object?)alumno.Monto ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@ObraSocialPrepaga", (object?)alumno.ObraSocialPrepaga ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DescripcionObraSocial", (object?)alumno.DescripcionObraSocial ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@TratamientoMedico", (object?)alumno.TratamientoMedico ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DescripcionTratamiento", (object?)alumno.DescripcionTratamiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Medicacion", (object?)alumno.Medicacion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DescripcionMedicacion", (object?)alumno.DescripcionMedicacion ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Discapacidad", (object?)alumno.Discapacidad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DescripcionDiscapacidad", (object?)alumno.DescripcionDiscapacidad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EstadoDiscapacidad", (object?)alumno.EstadoDiscapacidad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CertificadoDiscapacidad", (object?)alumno.CertificadoDiscapacidad ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@ContactoEmergencia", (object?)alumno.ContactoEmergencia ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TelefonoContacto", (object?)alumno.TelefonoContacto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FotoUrl", (object?)alumno.FotoUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", (object?)alumno.Activo ?? DBNull.Value);

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