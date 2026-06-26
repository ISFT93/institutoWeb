using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using instituto93.Domain.Models;
using instituto93.Data.Repositories.Interfaces;

namespace instituto93.Data.Repositories
{
    public class CursoRepository : icursoRepository
    {
        private readonly Conexion _conexion;

        public CursoRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<CursoModelo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<CursoModelo>();
            const string sql = "SELECT CursoId, NombreCurso, AnioCarreraId, Activo, AdmiteCurso FROM Cursos";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new CursoModelo
                    {
                        CursoId = reader.GetInt32(0),
                        NombreCurso = reader.GetString(1),
                        AnioCarreraId = reader.GetInt32(2),
                        Activo = reader.GetBoolean(3),
                        AdmiteCurso = reader.GetBoolean(4)
                    });
                }
            }

            finally
            {
                _conexion.Close();



            }
            return lista;
        }


        public async Task<CursoModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT CursoId, NombreCurso, AnioCarreraId, Activo, AdmiteCurso FROM Cursos WHERE CursoId = @id";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new CursoModelo
                    {
                        CursoId = reader.GetInt32(0),
                        NombreCurso = reader.GetString(1),
                        AnioCarreraId = reader.GetInt32(2),
                        Activo = reader.GetBoolean(3),
                        AdmiteCurso = reader.GetBoolean(4)
                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<int> CreateAsync(CursoModelo curso, CancellationToken cancellationToken = default)
        {
            if (curso == null) throw new ArgumentNullException(nameof(curso));
            const string sql = @"INSERT INTO Cursos (NombreCurso, AnioCarreraId, Activo, AdmiteCurso) 
                                 VALUES (@NombreCurso, @AnioCarreraId, @Activo, @AdmiteCurso); 
                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@NombreCurso", curso.NombreCurso ?? string.Empty);
                cmd.Parameters.AddWithValue("@AnioCarreraId", (object?)curso.AnioCarreraId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", curso.Activo);
                cmd.Parameters.AddWithValue("@AdmiteCurso", curso.AdmiteCurso);

                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(CursoModelo curso, CancellationToken cancellationToken = default)
        {
            if (curso == null) throw new ArgumentNullException(nameof(curso));
            const string sql = @"UPDATE Cursos 
                                 SET NombreCurso = @NombreCurso, 
                                     AnioCarreraId = @AnioCarreraId, 
                                     Activo = @Activo, 
                                     AdmiteCurso = @AdmiteCurso 
                                 WHERE CursoId = @CursoId";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@NombreCurso", curso.NombreCurso ?? string.Empty);
                cmd.Parameters.AddWithValue("@AnioCarreraId", (object?)curso.AnioCarreraId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", curso.Activo);
                cmd.Parameters.AddWithValue("@AdmiteCurso", curso.AdmiteCurso);
                cmd.Parameters.AddWithValue("@CursoId", curso.CursoId);

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
            const string sql = "DELETE FROM Cursos WHERE CursoId = @id";
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