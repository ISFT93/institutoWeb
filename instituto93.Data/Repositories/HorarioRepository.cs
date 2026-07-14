using instituto93.Domain.Interfaces;
using instituto93.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories
{
    //Mourdad ignacio
    public class HorarioRepository : IHorarioRepository
    {
        private readonly Conexion _conexion;

        public HorarioRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<IHorario>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<IHorario>();

            const string sql = @"
                SELECT 
                    H.HorarioId,
                    H.DiaId,
                    H.ModuloId,
                    H.CursoMateriaId,
                    CM.MateriaId,
                    CM.CursoId,
                    M.Nombre
                FROM Horarios H
                INNER JOIN CursoMaterias CM ON CM.CursoMateriaId = H.CursoMateriaId
                INNER JOIN Materias M ON M.MateriaId = CM.MateriaId
                ORDER BY H.HorarioId;
            ";

            try
            {
                await _conexion.OpenAsync(cancellationToken);

                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(MapearHorario(reader));
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<IHorario?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT 
                    H.HorarioId,
                    H.DiaId,
                    H.ModuloId,
                    H.CursoMateriaId,
                    CM.MateriaId,
                    CM.CursoId,
                    M.Nombre
                FROM Horarios H
                INNER JOIN CursoMaterias CM ON CM.CursoMateriaId = H.CursoMateriaId
                INNER JOIN Materias M ON M.MateriaId = CM.MateriaId
                WHERE H.HorarioId = @HorarioId;
            ";

            try
            {
                await _conexion.OpenAsync(cancellationToken);

                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.Add("@HorarioId", SqlDbType.Int).Value = id;

                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (await reader.ReadAsync(cancellationToken))
                {
                    return MapearHorario(reader);
                }

                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<IEnumerable<IHorario>> GetByCursoMateriaAsync(int cursoMateriaId, CancellationToken cancellationToken = default)
        {
            var lista = new List<IHorario>();

            const string sql = @"
                SELECT 
                    H.HorarioId,
                    H.DiaId,
                    H.ModuloId,
                    H.CursoMateriaId,
                    CM.MateriaId,
                    CM.CursoId,
                    M.Nombre
                FROM Horarios H
                INNER JOIN CursoMaterias CM ON CM.CursoMateriaId = H.CursoMateriaId
                INNER JOIN Materias M ON M.MateriaId = CM.MateriaId
                WHERE H.CursoMateriaId = @CursoMateriaId
                ORDER BY H.HorarioId;
            ";

            try
            {
                await _conexion.OpenAsync(cancellationToken);

                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.Add("@CursoMateriaId", SqlDbType.Int).Value = cursoMateriaId;

                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(MapearHorario(reader));
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<int> CreateAsync(IHorario horario, CancellationToken cancellationToken = default)
        {
            if (horario == null)
                throw new ArgumentNullException(nameof(horario));

            const string sql = @"
                INSERT INTO Horarios
                (
                    DiaId,
                    ModuloId,
                    CursoMateriaId
                )
                VALUES
                (
                    @DiaId,
                    @ModuloId,
                    @CursoMateriaId
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            try
            {
                await _conexion.OpenAsync(cancellationToken);

                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.Add("@DiaId", SqlDbType.Int).Value =
                    horario.DiaId.HasValue ? horario.DiaId.Value : DBNull.Value;

                cmd.Parameters.Add("@ModuloId", SqlDbType.Int).Value =
                    horario.ModuloId.HasValue ? horario.ModuloId.Value : DBNull.Value;

                cmd.Parameters.Add("@CursoMateriaId", SqlDbType.Int).Value = horario.CursoMateriaId;

                var result = await cmd.ExecuteScalarAsync(cancellationToken);

                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(IHorario horario, CancellationToken cancellationToken = default)
        {
            if (horario == null)
                throw new ArgumentNullException(nameof(horario));

            const string sql = @"
                UPDATE Horarios
                SET 
                    DiaId = @DiaId,
                    ModuloId = @ModuloId,
                    CursoMateriaId = @CursoMateriaId
                WHERE HorarioId = @HorarioId;
            ";

            try
            {
                await _conexion.OpenAsync(cancellationToken);

                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.Add("@DiaId", SqlDbType.Int).Value =
                    horario.DiaId.HasValue ? horario.DiaId.Value : DBNull.Value;

                cmd.Parameters.Add("@ModuloId", SqlDbType.Int).Value =
                    horario.ModuloId.HasValue ? horario.ModuloId.Value : DBNull.Value;

                cmd.Parameters.Add("@CursoMateriaId", SqlDbType.Int).Value = horario.CursoMateriaId;

                cmd.Parameters.Add("@HorarioId", SqlDbType.Int).Value = horario.HorarioId;

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
            const string sql = @"
                DELETE FROM Horarios
                WHERE HorarioId = @HorarioId;
            ";

            try
            {
                await _conexion.OpenAsync(cancellationToken);

                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.Add("@HorarioId", SqlDbType.Int).Value = id;

                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);

                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        private static IHorario MapearHorario(SqlDataReader reader)
        {
            return new IHorario
            {
                HorarioId = reader.GetInt32(reader.GetOrdinal("HorarioId")),

                DiaId = reader.IsDBNull(reader.GetOrdinal("DiaId"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("DiaId")),

                ModuloId = reader.IsDBNull(reader.GetOrdinal("ModuloId"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("ModuloId")),

                CursoMateriaId = reader.GetInt32(reader.GetOrdinal("CursoMateriaId")),

                MateriaId = reader.GetInt32(reader.GetOrdinal("MateriaId")),

                CursoId = reader.GetInt32(reader.GetOrdinal("CursoId")),

                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre"))
                    ? string.Empty
                    : reader.GetString(reader.GetOrdinal("Nombre"))
            };
        }
    }
}
