using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    public class TurnosRepository : ITurnosRepository
    {
        private readonly Conexion _conexion;
        public TurnosRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<TurnosLookup>> GetLookupAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<TurnosLookup>();
            const string sql = "SELECT TurnoId, Descripcion FROM Turnos ORDER BY TurnoId";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new TurnosLookup(
                        reader.GetInt32(0),
                        reader.GetString(1)));
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }
    }
}
