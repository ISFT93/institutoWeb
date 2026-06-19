using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    public class ParametrosRepository : IParametrosRepository
    {
        private readonly Conexion _conexion;

        public ParametrosRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<Parametros>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<Parametros>();
            const string sql = "SELECT ParametroId, Nombre, Descripcion, Valor, TipoId, Activo FROM Parametros";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(MapFromReader(reader));
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<Parametros?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT ParametroId, Nombre, Descripcion, Valor, TipoId, Activo FROM Parametros WHERE ParametroId = @ParametroId";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@ParametroId", id);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                    return MapFromReader(reader);
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        /// <summary>
        /// Equivalente a ObtenerParametro del DAO legacy. Devuelve el primer parámetro
        /// activo con el nombre indicado.
        /// </summary>
        public async Task<Parametros?> GetByNameAsync(string nombre, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT ParametroId, Nombre, Descripcion, Valor, TipoId, Activo "
                             + "FROM Parametros WHERE Nombre = @Nombre AND Activo = 1";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                    return MapFromReader(reader);
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<int> CreateAsync(Parametros parametros, CancellationToken cancellationToken = default)
        {
            if (parametros == null) throw new ArgumentNullException(nameof(parametros));
            const string sql = "INSERT INTO Parametros (Nombre, Descripcion, Valor, TipoId, Activo) "
                             + "VALUES (@Nombre, @Descripcion, @Valor, @TipoId, @Activo); "
                             + "SELECT CAST(SCOPE_IDENTITY() AS INT);";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Nombre", parametros.Nombre ?? string.Empty);
                cmd.Parameters.AddWithValue("@Descripcion", parametros.Descripcion ?? string.Empty);
                cmd.Parameters.AddWithValue("@Valor", parametros.Valor ?? string.Empty);
                cmd.Parameters.AddWithValue("@TipoId", parametros.TipoId);
                cmd.Parameters.AddWithValue("@Activo", parametros.Activo);
                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(Parametros parametros, CancellationToken cancellationToken = default)
        {
            if (parametros == null) throw new ArgumentNullException(nameof(parametros));
            const string sql = "UPDATE Parametros SET Valor = @Valor, Activo = @Activo WHERE ParametroId = @ParametroId";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Valor", parametros.Valor ?? string.Empty);
                cmd.Parameters.AddWithValue("@Activo", parametros.Activo);
                cmd.Parameters.AddWithValue("@ParametroId", parametros.ParametroId);
                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        /// <summary>
        /// Equivalente a ActualizarParametros del DAO legacy. Actualiza Valor y Activo
        /// de cada parámetro en la lista.
        /// </summary>
        public async Task<bool> UpdateRangeAsync(IEnumerable<Parametros> parametros, CancellationToken cancellationToken = default)
        {
            if (parametros == null) throw new ArgumentNullException(nameof(parametros));
            int rows = 0;
            const string sql = "UPDATE Parametros SET Valor = @Valor, Activo = @Activo WHERE ParametroId = @ParametroId";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                foreach (var parametro in parametros)
                {
                    using var cmd = _conexion.Conector.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@Valor", parametro.Valor ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Activo", parametro.Activo);
                    cmd.Parameters.AddWithValue("@ParametroId", parametro.ParametroId);
                    rows += await cmd.ExecuteNonQueryAsync(cancellationToken);
                }
            }
            finally
            {
                _conexion.Close();
            }

            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM Parametros WHERE ParametroId = @ParametroId";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@ParametroId", id);
                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        private static Parametros MapFromReader(System.Data.Common.DbDataReader reader) => new()
        {
            ParametroId = reader.GetInt32(0),
            Nombre      = reader.GetString(1),
            Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
            Valor       = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            TipoId      = reader.GetInt16(4),
            Activo      = reader.GetBoolean(5),
        };
    }
}
