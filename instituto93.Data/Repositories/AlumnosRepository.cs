using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace instituto93.Data.Repositories
//lopez melany
{
    public class AlumnosRepository : IAlumnosRepository
    {
        private readonly Conexion _conexion;

        public AlumnosRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<AlumnosModelo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<AlumnosModelo>();
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
                    lista.Add(new AlumnosModelo
                    {
                        AlumnoId = reader.IsDBNull(reader.GetOrdinal("AlumnoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AlumnoId")),
                        Apellido = reader.IsDBNull(reader.GetOrdinal("Apellido")) ? null : reader.GetString(reader.GetOrdinal("Apellido")),
                        Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                        TipoDocumento = reader.IsDBNull(reader.GetOrdinal("TipoDocumento")) ? null : reader.GetString(reader.GetOrdinal("TipoDocumento")),
                        NumeroDocumento = reader.IsDBNull(reader.GetOrdinal("NumeroDocumento")) ? null : reader.GetString(reader.GetOrdinal("NumeroDocumento")),
                        EstadoCivil = reader.IsDBNull(reader.GetOrdinal("EstadoCivil")) ? null : reader.GetString(reader.GetOrdinal("EstadoCivil")),
                        Sexo = reader.IsDBNull(reader.GetOrdinal("Sexo")) ? null : (char?)reader.GetString(reader.GetOrdinal("Sexo"))[0],
                        FechaNacimiento = reader.IsDBNull(reader.GetOrdinal("FechaNacimiento")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                        LocalidadNacimiento = reader.IsDBNull(reader.GetOrdinal("LocalidadNacimiento")) ? null : reader.GetString(reader.GetOrdinal("LocalidadNacimiento")),
                        PaisNacimiento = reader.IsDBNull(reader.GetOrdinal("PaisNacimiento")) ? null : reader.GetString(reader.GetOrdinal("PaisNacimiento")),
                        Calle = reader.IsDBNull(reader.GetOrdinal("Calle")) ? null : reader.GetString(reader.GetOrdinal("Calle")),
                        Numero = reader.IsDBNull(reader.GetOrdinal("Numero")) ? null : reader.GetString(reader.GetOrdinal("Numero")),
                        Piso = reader.IsDBNull(reader.GetOrdinal("Piso")) ? null : reader.GetString(reader.GetOrdinal("Piso")),
                        Departamento = reader.IsDBNull(reader.GetOrdinal("Departamento")) ? null : reader.GetString(reader.GetOrdinal("Departamento")),
                        Provincia = reader.IsDBNull(reader.GetOrdinal("Provincia")) ? null : reader.GetString(reader.GetOrdinal("Provincia")),
                        Distrito = reader.IsDBNull(reader.GetOrdinal("Distrito")) ? null : reader.GetString(reader.GetOrdinal("Distrito")),
                        Localidad = reader.IsDBNull(reader.GetOrdinal("Localidad")) ? null : reader.GetString(reader.GetOrdinal("Localidad")),
                        CodigoPostal = reader.IsDBNull(reader.GetOrdinal("CodigoPostal")) ? null : reader.GetString(reader.GetOrdinal("CodigoPostal")),
                        Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                        Celular = reader.IsDBNull(reader.GetOrdinal("Celular")) ? null : reader.GetString(reader.GetOrdinal("Celular")),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                        Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("Activo"))
                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<AlumnosModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
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
                    AlumnosModelo Map() => new AlumnosModelo
                    {
                        AlumnoId = reader.IsDBNull(reader.GetOrdinal("AlumnoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AlumnoId")),
                        Apellido = reader.IsDBNull(reader.GetOrdinal("Apellido")) ? null : reader.GetString(reader.GetOrdinal("Apellido")),
                        Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                        TipoDocumento = reader.IsDBNull(reader.GetOrdinal("TipoDocumento")) ? null : reader.GetString(reader.GetOrdinal("TipoDocumento")),
                        NumeroDocumento = reader.IsDBNull(reader.GetOrdinal("NumeroDocumento")) ? null : reader.GetString(reader.GetOrdinal("NumeroDocumento")),
                        EstadoCivil = reader.IsDBNull(reader.GetOrdinal("EstadoCivil")) ? null : reader.GetString(reader.GetOrdinal("EstadoCivil")),
                        Sexo = reader.IsDBNull(reader.GetOrdinal("Sexo")) ? null : (char?)reader.GetString(reader.GetOrdinal("Sexo"))[0],
                        FechaNacimiento = reader.IsDBNull(reader.GetOrdinal("FechaNacimiento")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                        LocalidadNacimiento = reader.IsDBNull(reader.GetOrdinal("LocalidadNacimiento")) ? null : reader.GetString(reader.GetOrdinal("LocalidadNacimiento")),
                        PaisNacimiento = reader.IsDBNull(reader.GetOrdinal("PaisNacimiento")) ? null : reader.GetString(reader.GetOrdinal("PaisNacimiento")),
                        Calle = reader.IsDBNull(reader.GetOrdinal("Calle")) ? null : reader.GetString(reader.GetOrdinal("Calle")),
                        Numero = reader.IsDBNull(reader.GetOrdinal("Numero")) ? null : reader.GetString(reader.GetOrdinal("Numero")),
                        Piso = reader.IsDBNull(reader.GetOrdinal("Piso")) ? null : reader.GetString(reader.GetOrdinal("Piso")),
                        Departamento = reader.IsDBNull(reader.GetOrdinal("Departamento")) ? null : reader.GetString(reader.GetOrdinal("Departamento")),
                        Provincia = reader.IsDBNull(reader.GetOrdinal("Provincia")) ? null : reader.GetString(reader.GetOrdinal("Provincia")),
                        Distrito = reader.IsDBNull(reader.GetOrdinal("Distrito")) ? null : reader.GetString(reader.GetOrdinal("Distrito")),
                        Localidad = reader.IsDBNull(reader.GetOrdinal("Localidad")) ? null : reader.GetString(reader.GetOrdinal("Localidad")),
                        CodigoPostal = reader.IsDBNull(reader.GetOrdinal("CodigoPostal")) ? null : reader.GetString(reader.GetOrdinal("CodigoPostal")),
                        Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                        Celular = reader.IsDBNull(reader.GetOrdinal("Celular")) ? null : reader.GetString(reader.GetOrdinal("Celular")),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                        TituloSecundario = reader.IsDBNull(reader.GetOrdinal("TituloSecundario")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("TituloSecundario")),
                        MateriasAdeuda = reader.IsDBNull(reader.GetOrdinal("MateriasAdeuda")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("MateriasAdeuda")),
                        DescripcionMaterias = reader.IsDBNull(reader.GetOrdinal("DescripcionMaterias")) ? null : reader.GetString(reader.GetOrdinal("DescripcionMaterias")),
                        Titulo = reader.IsDBNull(reader.GetOrdinal("Titulo")) ? null : reader.GetString(reader.GetOrdinal("Titulo")),
                        Orientacion = reader.IsDBNull(reader.GetOrdinal("Orientacion")) ? null : reader.GetString(reader.GetOrdinal("Orientacion")),
                        OtorgadoPor = reader.IsDBNull(reader.GetOrdinal("OtorgadoPor")) ? null : reader.GetString(reader.GetOrdinal("OtorgadoPor")),
                        AnioEgreso = reader.IsDBNull(reader.GetOrdinal("AnioEgreso")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("AnioEgreso")),
                        Promedio = reader.IsDBNull(reader.GetOrdinal("Promedio")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("Promedio")),
                        TituloTramite = reader.IsDBNull(reader.GetOrdinal("TituloTramite")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("TituloTramite")),
                        MayorTitulo = reader.IsDBNull(reader.GetOrdinal("MayorTitulo")) ? null : reader.GetString(reader.GetOrdinal("MayorTitulo")),
                        OtroTitulo = reader.IsDBNull(reader.GetOrdinal("OtroTitulo")) ? null : reader.GetString(reader.GetOrdinal("OtroTitulo")),
                        MayorOtorgadoPor = reader.IsDBNull(reader.GetOrdinal("MayorOtorgadoPor")) ? null : reader.GetString(reader.GetOrdinal("MayorOtorgadoPor")),
                        MayorPromedio = reader.IsDBNull(reader.GetOrdinal("MayorPromedio")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("MayorPromedio")),
                        FotocopiaTitulo = reader.IsDBNull(reader.GetOrdinal("FotocopiaTitulo")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("FotocopiaTitulo")),
                        ConstanciaTituloTramite = reader.IsDBNull(reader.GetOrdinal("ConstanciaTituloTramite")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("ConstanciaTituloTramite")),
                        ConstanciaAdeudaMaterias = reader.IsDBNull(reader.GetOrdinal("ConstanciaAdeudaMaterias")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("ConstanciaAdeudaMaterias")),
                        CantidadAdeudaMaterias = reader.IsDBNull(reader.GetOrdinal("CantidadAdeudaMaterias")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("CantidadAdeudaMaterias")),
                        CertificadoAptitud = reader.IsDBNull(reader.GetOrdinal("CertificadoAptitud")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("CertificadoAptitud")),
                        FotocopiaDocumento = reader.IsDBNull(reader.GetOrdinal("FotocopiaDocumento")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("FotocopiaDocumento")),
                        FotoCarnet = reader.IsDBNull(reader.GetOrdinal("FotoCarnet")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("FotoCarnet")),
                        FotocopiaPartidaNacimiento = reader.IsDBNull(reader.GetOrdinal("FotocopiaPartidaNacimiento")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("FotocopiaPartidaNacimiento")),
                        VacunaAntihepatitis = reader.IsDBNull(reader.GetOrdinal("VacunaAntihepatitis")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("VacunaAntihepatitis")),
                        VacunaAntitetanica = reader.IsDBNull(reader.GetOrdinal("VacunaAntitetanica")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("VacunaAntitetanica")),
                        Recibo = reader.IsDBNull(reader.GetOrdinal("Recibo")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("Recibo")),
                        Monto = reader.IsDBNull(reader.GetOrdinal("Monto")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("Monto")),
                        ObraSocialPrepaga = reader.IsDBNull(reader.GetOrdinal("ObraSocialPrepaga")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("ObraSocialPrepaga")),
                        DescripcionObraSocial = reader.IsDBNull(reader.GetOrdinal("DescripcionObraSocial")) ? null : reader.GetString(reader.GetOrdinal("DescripcionObraSocial")),
                        TratamientoMedico = reader.IsDBNull(reader.GetOrdinal("TratamientoMedico")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("TratamientoMedico")),
                        DescripcionTratamiento = reader.IsDBNull(reader.GetOrdinal("DescripcionTratamiento")) ? null : reader.GetString(reader.GetOrdinal("DescripcionTratamiento")),
                        Medicacion = reader.IsDBNull(reader.GetOrdinal("Medicacion")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("Medicacion")),
                        DescripcionMedicacion = reader.IsDBNull(reader.GetOrdinal("DescripcionMedicacion")) ? null : reader.GetString(reader.GetOrdinal("DescripcionMedicacion")),
                        Discapacidad = reader.IsDBNull(reader.GetOrdinal("Discapacidad")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("Discapacidad")),
                        DescripcionDiscapacidad = reader.IsDBNull(reader.GetOrdinal("DescripcionDiscapacidad")) ? null : reader.GetString(reader.GetOrdinal("DescripcionDiscapacidad")),
                        EstadoDiscapacidad = reader.IsDBNull(reader.GetOrdinal("EstadoDiscapacidad")) ? null : reader.GetString(reader.GetOrdinal("EstadoDiscapacidad")),
                        CertificadoDiscapacidad = reader.IsDBNull(reader.GetOrdinal("CertificadoDiscapacidad")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("CertificadoDiscapacidad")),
                        ContactoEmergencia = reader.IsDBNull(reader.GetOrdinal("ContactoEmergencia")) ? null : reader.GetString(reader.GetOrdinal("ContactoEmergencia")),
                        TelefonoContacto = reader.IsDBNull(reader.GetOrdinal("TelefonoContacto")) ? null : reader.GetString(reader.GetOrdinal("TelefonoContacto")),
                        Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("Activo")),
                        FotoUrl = reader.IsDBNull(reader.GetOrdinal("FotoUrl")) ? null : reader.GetString(reader.GetOrdinal("FotoUrl")),
                        Carrera = reader.IsDBNull(reader.GetOrdinal("Carrera")) ? null : reader.GetString(reader.GetOrdinal("Carrera"))
                    };

                    return Map();
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<int> CreateAsync(AlumnosModelo alumnosModelo, CancellationToken cancellationToken = default)
        {
            if (alumnosModelo == null) throw new ArgumentNullException(nameof(alumnosModelo));

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
                cmd.Parameters.AddWithValue("@Apellido", (object?)alumnosModelo.Apellido ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nombre", (object?)alumnosModelo.Nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoDocumento", (object?)alumnosModelo.TipoDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NumeroDocumento", (object?)alumnosModelo.NumeroDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaNacimiento", (object?)alumnosModelo.FechaNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", (object?)alumnosModelo.Activo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Carrera", (object?)alumnosModelo.Carrera ?? DBNull.Value);

                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(AlumnosModelo alumnosModelo, CancellationToken cancellationToken = default)
        {
            if (alumnosModelo == null) throw new ArgumentNullException(nameof(alumnosModelo));

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
                cmd.Parameters.AddWithValue("@Apellido", (object?)alumnosModelo.Apellido ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nombre", (object?)alumnosModelo.Nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoDocumento", (object?)alumnosModelo.TipoDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NumeroDocumento", (object?)alumnosModelo.NumeroDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaNacimiento", (object?)alumnosModelo.FechaNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", (object?)alumnosModelo.Activo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Carrera", (object?)alumnosModelo.Carrera ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", alumnosModelo.AlumnoId);

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