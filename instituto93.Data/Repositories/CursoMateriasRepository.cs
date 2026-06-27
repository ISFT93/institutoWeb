using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories
{
    public class CursoMateriasRepository : ICursoMateriasRepository
    {
        private readonly Conexion _conexion;

        public CursoMateriasRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<CursoMaterias>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<CursoMaterias>();

            const string sql = @"SELECT CursoMateriaId,
                                        MateriaId,
                                        CursoId,
                                        FechaAlta,
                                        FechaBaja,
                                        Activo
                                 FROM CursoMaterias";

            try
            {
                await _conexion.OpenAsync(cancellationToken);

                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new CursoMaterias
                    {
                        CursoMateriaId = reader["CursoMateriaId"].ToString(),
                        MateriaId = reader["MateriaId"].ToString(),
                        CursoId = reader["CursoId"].ToString(),
                        FechaAlta = reader["FechaAlta"].ToString(),
                        FechaBaja = reader["FechaBaja"].ToString(),
                        Activo = reader["Activo"].ToString()
                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<CursoMaterias?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT CursoMateriaId,
                                        MateriaId,
                                        CursoId,
                                        FechaAlta,
                                        FechaBaja,
                                        Activo
                                 FROM CursoMaterias
                                 WHERE CursoMateriaId = @id";

            try
            {
                await _conexion.OpenAsync(cancellationToken);

                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (await reader.ReadAsync(cancellationToken))
                {
                    return new CursoMaterias
                    {
                        CursoMateriaId = reader["CursoMateriaId"].ToString(),
                        MateriaId = reader["MateriaId"].ToString(),
                        CursoId = reader["CursoId"].ToString(),
                        FechaAlta = reader["FechaAlta"].ToString(),
                        FechaBaja = reader["FechaBaja"].ToString(),
                        Activo = reader["Activo"].ToString()
                    };
                }

                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<int> CreateAsync(CursoMaterias cursoMaterias, CancellationToken cancellationToken = default)
        {
            if (cursoMaterias == null)
                throw new ArgumentNullException(nameof(cursoMaterias));

            const string sql = @"
                INSERT INTO CursoMaterias
                (
                    MateriaId,
                    CursoId,
                    FechaAlta,
                    FechaBaja,
                    Activo
                )
                VALUES
                (
                    @MateriaId,
                    @CursoId,
                    @FechaAlta,
                    @FechaBaja,
                    @Activo
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            try
            {
                await _conexion.OpenAsync(cancellationToken);

                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@MateriaId", cursoMaterias.MateriaId);
                cmd.Parameters.AddWithValue("@CursoId", cursoMaterias.CursoId);
                cmd.Parameters.AddWithValue("@FechaAlta", cursoMaterias.FechaAlta);
                cmd.Parameters.AddWithValue("@FechaBaja", cursoMaterias.FechaBaja);
                cmd.Parameters.AddWithValue("@Activo", cursoMaterias.Activo);

                var result = await cmd.ExecuteScalarAsync(cancellationToken);

                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(CursoMaterias cursoMaterias, CancellationToken cancellationToken = default)
        {
            if (cursoMaterias == null)
                throw new ArgumentNullException(nameof(cursoMaterias));

            const string sql = @"
                UPDATE CursoMaterias
                SET MateriaId = @MateriaId,
                    CursoId = @CursoId,
                    FechaAlta = @FechaAlta,
                    FechaBaja = @FechaBaja,
                    Activo = @Activo
                WHERE CursoMateriaId = @CursoMateriaId";

            try
            {
                await _conexion.OpenAsync(cancellationToken);

                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@CursoMateriaId", cursoMaterias.CursoMateriaId);
                cmd.Parameters.AddWithValue("@MateriaId", cursoMaterias.MateriaId);
                cmd.Parameters.AddWithValue("@CursoId", cursoMaterias.CursoId);
                cmd.Parameters.AddWithValue("@FechaAlta", cursoMaterias.FechaAlta);
                cmd.Parameters.AddWithValue("@FechaBaja", cursoMaterias.FechaBaja);
                cmd.Parameters.AddWithValue("@Activo", cursoMaterias.Activo);

                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);

                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            const string sql = @"DELETE FROM CursoMaterias
                                 WHERE CursoMateriaId = @id";

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
