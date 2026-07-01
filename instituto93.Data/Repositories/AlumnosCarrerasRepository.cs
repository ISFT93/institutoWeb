using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using instituto93.Domain.Models;
using instituto93.Data.Repositories.Interfaces;

namespace instituto93.Data.Repositories
{
    // Acevedo Cecilia
    public class AlumnosCarrerasRepository : IAlumnosCarrerasRepository
    {
        private readonly Conexion _conexion;

        //El Constructor es un método especial que se llama automáticamente cuando se crea una instancia de la clase.
        //En este caso, el constructor de AlumnosCarrerasRepository recibe un objeto de tipo Conexion como parámetro
        //y lo asigna a la variable privada _conexion. Esto permite que la clase tenga acceso a la conexión a la base de datos
        //para realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) en la tabla correspondiente a AlumnosCarreras.
        public AlumnosCarrerasRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        // TRAE TODOS LOS REGISTROS DE LA TABLA (EN ESTE CASO, TODAS LAS RELACIONES ENTRE ALUMNOS Y CARRERAS)
        public async Task<IEnumerable<AlumnosCarreras>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<AlumnosCarreras>();
            // Cambiamos la consulta para la tabla AlumnosCarreras
            const string sql = "SELECT AlumnoCarreraId, CarreraId, AlumnoId, FechaAlta, FechaBaja, Activo FROM AlumnosCarreras";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    // Mapeamos las columnas del modelo AlumnosCarreras
                    lista.Add(new AlumnosCarreras
                    {
                        AlumnoCarreraId = reader.GetInt32(0),
                        CarreraId = reader.GetInt32(1),
                        AlumnoId = reader.GetInt32(2),
                        // Las fechas pueden ser NULL, por eso usamos IsDBNull antes de leerlas
                        FechaAlta = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                        FechaBaja = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                        Activo = reader.GetBoolean(5)
                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        // TRAER DATOS SOLO POR ID (EN ESTE CASO, EL ID DE LA RELACIÓN ENTRE ALUMNO Y CARRERA)
        public async Task<AlumnosCarreras?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT AlumnoCarreraId, CarreraId, AlumnoId, FechaAlta, FechaBaja, Activo " +
                               "FROM AlumnosCarreras WHERE AlumnoCarreraId = @id";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new AlumnosCarreras
                    {
                        AlumnoCarreraId = reader.GetInt32(0),
                        CarreraId = reader.GetInt32(1),
                        AlumnoId = reader.GetInt32(2),
                        FechaAlta = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                        FechaBaja = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                        Activo = reader.GetBoolean(5)

                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        // CREAR UN REGISTRO (EN ESTE CASO, CREAMOS UNA RELACIÓN ENTRE UN ALUMNO Y UNA CARRERA)
        public async Task<int> CreateAsync(AlumnosCarreras alumnoCarrera, CancellationToken cancellationToken = default)
        {
            if (alumnoCarrera == null) throw new ArgumentNullException(nameof(alumnoCarrera));

            // Insertamos el AlumnoId e CarreraId que vienen en el modelo AlumnosCarreras
            const string sql = @"INSERT INTO AlumnosCarreras (CarreraId, AlumnoId, FechaAlta, FechaBaja, Activo) 
                                 VALUES (@CarreraId, @AlumnoId, @FechaAlta, @FechaBaja, @Activo); 
                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                // Pasamos los parámetros necesarios
                cmd.Parameters.AddWithValue("@CarreraId", alumnoCarrera.CarreraId);
                cmd.Parameters.AddWithValue("@AlumnoId", alumnoCarrera.AlumnoId);
                // DBNull.Value le avisa a SQL que mandamos un valor nulo si la fecha no está seteada
                cmd.Parameters.AddWithValue("@FechaAlta", (object?)alumnoCarrera.FechaAlta ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaBaja", (object?)alumnoCarrera.FechaBaja ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", alumnoCarrera.Activo);

                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        // ACTUALIZÁ UN REGISTRO (EN ESTE CASO, ACTUALIZAMOS LA RELACIÓN
        // ENTRE UN ALUMNO Y UNA CARRERA, POR EJEMPLO, CAMBIANDO LA FECHA DE BAJA O EL ESTADO DE ACTIVO)
        public async Task<bool> UpdateAsync(AlumnosCarreras alumnoCarrera, CancellationToken cancellationToken = default)
        {
            if (alumnoCarrera == null) throw new ArgumentNullException(nameof(alumnoCarrera));

            const string sql = @"UPDATE AlumnosCarreras 
                                 SET CarreraId = @CarreraId, AlumnoId = @AlumnoId, FechaAlta = @FechaAlta, FechaBaja = @FechaBaja, Activo = @Activo 
                                 WHERE AlumnoCarreraId = @AlumnoCarreraId";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@CarreraId", alumnoCarrera.CarreraId);
                cmd.Parameters.AddWithValue("@AlumnoId", alumnoCarrera.AlumnoId);
                cmd.Parameters.AddWithValue("@FechaAlta", (object?)alumnoCarrera.FechaAlta ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaBaja", (object?)alumnoCarrera.FechaBaja ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", alumnoCarrera.Activo);
                cmd.Parameters.AddWithValue("@AlumnoCarreraId", alumnoCarrera.AlumnoCarreraId);

                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        // ELIMINAR UN REGISTRO (EN ESTE CASO, ELIMINAMOS LA RELACIÓN ENTRE UN ALUMNO Y UNA CARRERA)
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM AlumnosCarreras WHERE id = @id";
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
