using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    public class CargosRepository : ICargosRepository
    {
        private readonly Conexion _conexion;

        public CargosRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }
        public async Task<IEnumerable<CargosModelo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<CargosModelo>();
            const string sql = "SELECT CargoId, Descripcion, Activo, TipoAsignacionId, TipoAplicacionId FROM Cargos";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new CargosModelo
                    {
                        CargoId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                        Activo = reader.GetBoolean(2),
                        TipoAsignacionId = reader.GetInt32(3),
                        TipoAplicacionId = reader.GetInt32(4)
                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }
        public async Task<CargosModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT CargoId, Descripcion, Activo, TipoAsignacionId, TipoAplicacionId FROM Cargos WHERE id = @id";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new CargosModelo
                    {
                        CargoId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                        Activo = reader.GetBoolean(2),
                        TipoAsignacionId = reader.GetInt32(3),
                        TipoAplicacionId = reader.GetInt32(4)
                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }
        public async Task<int> CreateAsync(CargosModelo cargo, CancellationToken cancellationToken = default)
        {
            if (cargo == null) throw new ArgumentNullException(nameof(cargo));
            const string sql = "INSERT INTO Cargos (Descripcion, Activo, TipoAsignacionId, TipoAplicacionId) VALUES (@descripcion, @Activo, @TipoAsignacionId, @TipoAplicacionId); SELECT CAST(SCOPE_IDENTITY() AS INT)";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@descripcion", cargo.Descripcion ?? string.Empty);
                cmd.Parameters.AddWithValue("@Activo", cargo.Activo);
                cmd.Parameters.AddWithValue("@TipoAsignacionId", cargo.TipoAsignacionId);
                cmd.Parameters.AddWithValue("@TipoAplicacionId", cargo.TipoAplicacionId);
                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public async Task<bool> UpdateAsync(CargosModelo cargo, CancellationToken cancellationToken = default)
        {
            if (cargo == null) throw new ArgumentNullException(nameof(cargo));
            const string sql = "UPDATE Cargos SET Descripcion = @descripcion, Activo = @Activo, TipoAsignacionId = @TipoAsignacionId, TipoAplicacionId = @TipoAplicacionId WHERE CargoId = @id";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.Parameters.AddWithValue("@id", cargo.CargoId);
                cmd.Parameters.AddWithValue("@descripcion", cargo.Descripcion ?? string.Empty);
                cmd.Parameters.AddWithValue("@Activo", cargo.Activo);
                cmd.Parameters.AddWithValue("@TipoAsignacionId", cargo.TipoAsignacionId);
                cmd.Parameters.AddWithValue("@TipoAplicacionId", cargo.TipoAplicacionId);
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
            const string sql = "DELETE FROM Cargos WHERE CargoId = @id";
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