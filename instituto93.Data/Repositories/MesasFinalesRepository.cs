using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using System.Data;

namespace instituto93.Data.Repositories
{
    public class MesasFinalesRepository : IMesasFinalesRepository
    {
        private readonly Conexion _conexion;
        public MesasFinalesRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<MesaFinalFila>> GetListAsync(MesasFinalesFiltro filtro, CancellationToken cancellationToken = default)
        {
            var lista = new List<MesaFinalFila>();

            const string sql = @"
                SELECT
                    mf.MesaFinalId,
                    ISNULL(m.Nombre, ''),
                    ac.AnioCarrera,
                    ISNULL(voc.Nombre + ' ' + voc.Apellido, ''),
                    ISNULL(pre.Nombre + ' ' + pre.Apellido, ''),
                    mf.Fecha,
                    ISNULL(s.Descripcion, ''),
                    CASE WHEN ma.MesaAlumnoId IS NOT NULL THEN 1 ELSE 0 END
                FROM MesasFinales mf
                INNER JOIN Materias m       ON m.MateriaId = mf.MateriaId
                INNER JOIN AniosCarreras ac ON ac.AnioCarreraId = m.AnioCarreraId
                INNER JOIN Personal voc     ON voc.PersonalId = mf.VocalId
                INNER JOIN Personal pre     ON pre.PersonalId = mf.PresidenteId
                INNER JOIN Sedes s          ON s.SedeId = mf.SedeId
                LEFT JOIN MesaAlumno ma     ON ma.MesaFinalId = mf.MesaFinalId AND ma.AlumnoId = @AlumnoId
                WHERE (@CarreraId IS NULL OR mf.CarreraId = @CarreraId)
                  AND (@TurnoId IS NULL OR mf.TurnoId = @TurnoId)
                  AND (@AnioCarreraId IS NULL OR ac.AnioCarreraId = @AnioCarreraId)
                  AND (@AnioLectivo IS NULL OR mf.CicloLectivoId = @AnioLectivo)
                  AND mf.Fecha IS NOT NULL
                  AND (@Texto IS NULL
                       OR m.Nombre LIKE @Texto
                       OR ISNULL(voc.Nombre + ' ' + voc.Apellido, '') LIKE @Texto
                       OR ISNULL(pre.Nombre + ' ' + pre.Apellido, '') LIKE @Texto
                       OR ISNULL(s.Descripcion, '') LIKE @Texto)
                ORDER BY ac.AnioCarrera, mf.Fecha;
            ";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@CarreraId", SqlDbType.Int).Value = (object?)filtro.CarreraId ?? DBNull.Value;
                cmd.Parameters.Add("@TurnoId", SqlDbType.Int).Value = (object?)filtro.TurnoId ?? DBNull.Value;
                cmd.Parameters.Add("@AnioCarreraId", SqlDbType.Int).Value = (object?)filtro.AnioCarreraId ?? DBNull.Value;
                cmd.Parameters.Add("@AnioLectivo", SqlDbType.Int).Value = (object?)filtro.AnioLectivo ?? DBNull.Value;
                cmd.Parameters.Add("@AlumnoId", SqlDbType.Int).Value = (object?)filtro.AlumnoId ?? DBNull.Value;
                cmd.Parameters.Add("@Texto", SqlDbType.VarChar, 200).Value =
                    string.IsNullOrWhiteSpace(filtro.Texto) ? DBNull.Value : $"%{filtro.Texto.Trim()}%";

                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new MesaFinalFila(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                        reader.GetString(6),
                        reader.GetInt32(7) == 1));
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
