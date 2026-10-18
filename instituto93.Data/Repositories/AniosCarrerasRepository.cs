using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    public class AniosCarrerasRepository : IAniosCarrerasRepository
    {
        private readonly Conexion _conexion;
        public AniosCarrerasRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<AnioCarreraLookup>> GetLookupAsync(int carreraId, CancellationToken cancellationToken = default)
        {
            var lista = new List<AnioCarreraLookup>();
            const string sql = "SELECT AnioCarreraId, AnioCarrera FROM AniosCarreras WHERE CarreraId = @CarreraId ORDER BY AnioCarrera";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@CarreraId", carreraId);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new AnioCarreraLookup(
                        reader.GetInt32(0),
                        reader.GetInt32(1)));
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
