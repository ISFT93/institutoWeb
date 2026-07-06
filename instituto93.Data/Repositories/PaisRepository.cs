using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public class PaisRepository : IPaisRepository
    {
        private readonly Conexion _conexion;

        public PaisRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<Pais>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<Pais>();
            const string sql = "SELECT id, nombre, abreviatura FROM Paises";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new Pais
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Abreviatura = reader.GetString(2)
                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<Pais?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT id, nombre, abreviatura FROM Paises WHERE id = @id";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new Pais
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Abreviatura = reader.GetString(2)
                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<int> CreateAsync(Pais pais, CancellationToken cancellationToken = default)
        {
            if (pais == null) throw new ArgumentNullException(nameof(pais));
            const string sql = "INSERT INTO Paises (nombre, abreviatura) VALUES (@nombre, @abreviatura); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@nombre", pais.Nombre ?? string.Empty);
                cmd.Parameters.AddWithValue("@abreviatura", pais.Abreviatura ?? string.Empty);
                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(Pais pais, CancellationToken cancellationToken = default)
        {
            if (pais == null) throw new ArgumentNullException(nameof(pais));
            const string sql = "UPDATE Paises SET nombre = @nombre, abreviatura = @abreviatura WHERE id = @id";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@nombre", pais.Nombre ?? string.Empty);
                cmd.Parameters.AddWithValue("@abreviatura", pais.Abreviatura ?? string.Empty);
                cmd.Parameters.AddWithValue("@id", pais.Id);
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
            const string sql = "DELETE FROM Paises WHERE id = @id";
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