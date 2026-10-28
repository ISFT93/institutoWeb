using instituto93.Data.Repositories.Interfaces;
using System.Data;

namespace instituto93.Data.Repositories
{
    public class MesasFinalesAlumnosRepository : IMesasFinalesAlumnosRepository
    {
        private readonly Conexion _conexion;
        public MesasFinalesAlumnosRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<List<int>> GetInscritosMesaIdsAsync(int alumnoId, IEnumerable<int> mesaFinalIds, CancellationToken cancellationToken = default)
        {
            var ids = mesaFinalIds?.Distinct().ToList() ?? new List<int>();
            if (ids.Count == 0) return new List<int>();

            var lista = new List<int>();
            var parametros = string.Join(",", ids.Select((_, i) => $"@Id{i}"));
            var sql = $@"SELECT MesaFinalId FROM dbo.MesaAlumno WHERE AlumnoId = @AlumnoId AND MesaFinalId IN ({parametros});";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@AlumnoId", SqlDbType.Int).Value = alumnoId;
                for (int i = 0; i < ids.Count; i++)
                    cmd.Parameters.Add($"@Id{i}", SqlDbType.Int).Value = ids[i];

                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                    lista.Add(reader.GetInt32(0));
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<int> InscribirAsync(int alumnoId, IEnumerable<int> mesaFinalIds, CancellationToken cancellationToken = default)
        {
            var ids = mesaFinalIds?.Distinct().ToList() ?? new List<int>();
            if (ids.Count == 0) return 0;

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                var parametros = string.Join(",", ids.Select((_, i) => $"(@MesaId{i})"));
                cmd.CommandText = $@"INSERT INTO dbo.MesaAlumno (AlumnoId, MesaFinalId) SELECT @AlumnoId, v.MesaFinalId FROM (VALUES {parametros}) AS v(MesaFinalId) WHERE NOT EXISTS (SELECT 1 FROM dbo.MesaAlumno ma WHERE ma.AlumnoId = @AlumnoId AND ma.MesaFinalId = v.MesaFinalId);";
                cmd.Parameters.Add("@AlumnoId", SqlDbType.Int).Value = alumnoId;
                for (int i = 0; i < ids.Count; i++)
                    cmd.Parameters.Add($"@MesaId{i}", SqlDbType.Int).Value = ids[i];

                return await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
            finally
            {
                _conexion.Close();
            }
        }
    }
}
