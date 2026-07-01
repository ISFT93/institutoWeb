using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    public class CicloLectivoRepository : ICicloLectivoRepository
    {
        private readonly Conexion _conexion;

        public CicloLectivoRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<CicloLectivoModelo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<CicloLectivoModelo>();
            const string sql = @"SELECT AnioLectivo, CantidadSemana, FechaInicio, FechaInscripcionInicio, 
                                 FechaInscripcionFinal, FechaMarzoInicio, FechaMarzoFinal, FechaJunioInicio, 
                                 FechaJunioFinal, FechaDiciembreInicio, FechaDiciembreFinal, FechaEspecialInicio, 
                                 FechaEspecialFinal, FechaCierre, Activo, FechaPreInscripcionInicio, 
                                 FechaPreInscripcionFinal FROM CicloLectivo";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new CicloLectivoModelo
                    {
                        AnioLectivo = reader.GetInt32(reader.GetOrdinal("AnioLectivo")),
                        CantidadSemana = reader.GetInt32(reader.GetOrdinal("CantidadSemana")),
                        FechaInicio = reader.IsDBNull(reader.GetOrdinal("FechaInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                        FechaInscripcionInicio = reader.IsDBNull(reader.GetOrdinal("FechaInscripcionInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaInscripcionInicio")),
                        FechaInscripcionFinal = reader.IsDBNull(reader.GetOrdinal("FechaInscripcionFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaInscripcionFinal")),
                        FechaMarzoInicio = reader.IsDBNull(reader.GetOrdinal("FechaMarzoInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaMarzoInicio")),
                        FechaMarzoFinal = reader.IsDBNull(reader.GetOrdinal("FechaMarzoFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaMarzoFinal")),
                        FechaJunioInicio = reader.IsDBNull(reader.GetOrdinal("FechaJunioInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaJunioInicio")),
                        FechaJunioFinal = reader.IsDBNull(reader.GetOrdinal("FechaJunioFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaJunioFinal")),
                        FechaDiciembreInicio = reader.IsDBNull(reader.GetOrdinal("FechaDiciembreInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaDiciembreInicio")),
                        FechaDiciembreFinal = reader.IsDBNull(reader.GetOrdinal("FechaDiciembreFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaDiciembreFinal")),
                        FechaEspecialInicio = reader.IsDBNull(reader.GetOrdinal("FechaEspecialInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaEspecialInicio")),
                        FechaEspecialFinal = reader.IsDBNull(reader.GetOrdinal("FechaEspecialFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaEspecialFinal")),
                        FechaCierre = reader.IsDBNull(reader.GetOrdinal("FechaCierre")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaCierre")),
                        Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? null : reader.GetBoolean(reader.GetOrdinal("Activo")),
                        FechaPreInscripcionInicio = reader.IsDBNull(reader.GetOrdinal("FechaPreInscripcionInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaPreInscripcionInicio")),
                        FechaPreInscripcionFinal = reader.IsDBNull(reader.GetOrdinal("FechaPreInscripcionFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaPreInscripcionFinal"))
                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<CicloLectivoModelo?> GetByAnioAsync(int anioLectivo, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT AnioLectivo, CantidadSemana, FechaInicio, FechaInscripcionInicio, 
                                 FechaInscripcionFinal, FechaMarzoInicio, FechaMarzoFinal, FechaJunioInicio, 
                                 FechaJunioFinal, FechaDiciembreInicio, FechaDiciembreFinal, FechaEspecialInicio, 
                                 FechaEspecialFinal, FechaCierre, Activo, FechaPreInscripcionInicio, 
                                 FechaPreInscripcionFinal FROM CicloLectivo WHERE AnioLectivo = @AnioLectivo";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@AnioLectivo", anioLectivo);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new CicloLectivoModelo
                    {
                        AnioLectivo = reader.GetInt32(reader.GetOrdinal("AnioLectivo")),
                        CantidadSemana = reader.GetInt32(reader.GetOrdinal("CantidadSemana")),
                        FechaInicio = reader.IsDBNull(reader.GetOrdinal("FechaInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                        FechaInscripcionInicio = reader.IsDBNull(reader.GetOrdinal("FechaInscripcionInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaInscripcionInicio")),
                        FechaInscripcionFinal = reader.IsDBNull(reader.GetOrdinal("FechaInscripcionFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaInscripcionFinal")),
                        FechaMarzoInicio = reader.IsDBNull(reader.GetOrdinal("FechaMarzoInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaMarzoInicio")),
                        FechaMarzoFinal = reader.IsDBNull(reader.GetOrdinal("FechaMarzoFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaMarzoFinal")),
                        FechaJunioInicio = reader.IsDBNull(reader.GetOrdinal("FechaJunioInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaJunioInicio")),
                        FechaJunioFinal = reader.IsDBNull(reader.GetOrdinal("FechaJunioFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaJunioFinal")),
                        FechaDiciembreInicio = reader.IsDBNull(reader.GetOrdinal("FechaDiciembreInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaDiciembreInicio")),
                        FechaDiciembreFinal = reader.IsDBNull(reader.GetOrdinal("FechaDiciembreFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaDiciembreFinal")),
                        FechaEspecialInicio = reader.IsDBNull(reader.GetOrdinal("FechaEspecialInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaEspecialInicio")),
                        FechaEspecialFinal = reader.IsDBNull(reader.GetOrdinal("FechaEspecialFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaEspecialFinal")),
                        FechaCierre = reader.IsDBNull(reader.GetOrdinal("FechaCierre")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaCierre")),
                        Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? null : reader.GetBoolean(reader.GetOrdinal("Activo")),
                        FechaPreInscripcionInicio = reader.IsDBNull(reader.GetOrdinal("FechaPreInscripcionInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaPreInscripcionInicio")),
                        FechaPreInscripcionFinal = reader.IsDBNull(reader.GetOrdinal("FechaPreInscripcionFinal")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaPreInscripcionFinal"))
                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<int> CreateAsync(CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken = default)
        {
            if (cicloLectivo == null) throw new ArgumentNullException(nameof(cicloLectivo));

            const string sql = @"INSERT INTO CicloLectivo (AnioLectivo, CantidadSemana, FechaInicio, FechaInscripcionInicio, 
                                 FechaInscripcionFinal, FechaMarzoInicio, FechaMarzoFinal, FechaJunioInicio, 
                                 FechaJunioFinal, FechaDiciembreInicio, FechaDiciembreFinal, FechaEspecialInicio, 
                                 FechaEspecialFinal, FechaCierre, FechaPreInscripcionInicio, FechaPreInscripcionFinal) 
                                 VALUES (@AnioLectivo, @CantidadSemana, @FechaInicio, @FechaInscripcionInicio, 
                                 @FechaInscripcionFinal, @FechaMarzoInicio, @FechaMarzoFinal, @FechaJunioInicio, 
                                 @FechaJunioFinal, @FechaDiciembreInicio, @FechaDiciembreFinal, @FechaEspecialInicio, 
                                 @FechaEspecialFinal, @FechaCierre, @FechaPreInscripcionInicio, @FechaPreInscripcionFinal)";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@AnioLectivo", cicloLectivo.AnioLectivo);
                cmd.Parameters.AddWithValue("@CantidadSemana", cicloLectivo.CantidadSemana);
                cmd.Parameters.AddWithValue("@FechaInicio", (object)cicloLectivo.FechaInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaInscripcionInicio", (object)cicloLectivo.FechaInscripcionInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaInscripcionFinal", (object)cicloLectivo.FechaInscripcionFinal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaMarzoInicio", (object)cicloLectivo.FechaMarzoInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaMarzoFinal", (object)cicloLectivo.FechaMarzoFinal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaJunioInicio", (object)cicloLectivo.FechaJunioInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaJunioFinal", (object)cicloLectivo.FechaJunioFinal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaDiciembreInicio", (object)cicloLectivo.FechaDiciembreInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaDiciembreFinal", (object)cicloLectivo.FechaDiciembreFinal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaEspecialInicio", (object)cicloLectivo.FechaEspecialInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaEspecialFinal", (object)cicloLectivo.FechaEspecialFinal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaCierre", (object)cicloLectivo.FechaCierre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaPreInscripcionInicio", (object)cicloLectivo.FechaPreInscripcionInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaPreInscripcionFinal", (object)cicloLectivo.FechaPreInscripcionFinal ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync(cancellationToken);
                return cicloLectivo.AnioLectivo;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken = default)
        {
            if (cicloLectivo == null) throw new ArgumentNullException(nameof(cicloLectivo));

            const string sql = @"UPDATE CicloLectivo SET 
                                 CantidadSemana = @CantidadSemana, FechaInicio = @FechaInicio, 
                                 FechaInscripcionInicio = @FechaInscripcionInicio, FechaInscripcionFinal = @FechaInscripcionFinal, 
                                 FechaMarzoInicio = @FechaMarzoInicio, FechaMarzoFinal = @FechaMarzoFinal, 
                                 FechaJunioInicio = @FechaJunioInicio, FechaJunioFinal = @FechaJunioFinal, 
                                 FechaDiciembreInicio = @FechaDiciembreInicio, FechaDiciembreFinal = @FechaDiciembreFinal, 
                                 FechaEspecialInicio = @FechaEspecialInicio, FechaEspecialFinal = @FechaEspecialFinal, 
                                 FechaCierre = @FechaCierre, FechaPreInscripcionInicio = @FechaPreInscripcionInicio, 
                                 FechaPreInscripcionFinal = @FechaPreInscripcionFinal 
                                 WHERE AnioLectivo = @AnioLectivo";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@AnioLectivo", cicloLectivo.AnioLectivo);
                cmd.Parameters.AddWithValue("@CantidadSemana", cicloLectivo.CantidadSemana);
                cmd.Parameters.AddWithValue("@FechaInicio", (object)cicloLectivo.FechaInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaInscripcionInicio", (object)cicloLectivo.FechaInscripcionInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaInscripcionFinal", (object)cicloLectivo.FechaInscripcionFinal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaMarzoInicio", (object)cicloLectivo.FechaMarzoInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaMarzoFinal", (object)cicloLectivo.FechaMarzoFinal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaJunioInicio", (object)cicloLectivo.FechaJunioInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaJunioFinal", (object)cicloLectivo.FechaJunioFinal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaDiciembreInicio", (object)cicloLectivo.FechaDiciembreInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaDiciembreFinal", (object)cicloLectivo.FechaDiciembreFinal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaEspecialInicio", (object)cicloLectivo.FechaEspecialInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaEspecialFinal", (object)cicloLectivo.FechaEspecialFinal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaCierre", (object)cicloLectivo.FechaCierre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaPreInscripcionInicio", (object)cicloLectivo.FechaPreInscripcionInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaPreInscripcionFinal", (object)cicloLectivo.FechaPreInscripcionFinal ?? DBNull.Value);

                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> DeleteAsync(int anioLectivo, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM CicloLectivo WHERE AnioLectivo = @AnioLectivo";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@AnioLectivo", anioLectivo);
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