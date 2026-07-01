using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories
{
    public class LibroActasRepository : ILibrosActasRepository
    {
        private readonly Conexion _conexion;

        public LibroActasRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<LibroActasModelo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<LibroActasModelo>();
            const string sql = "SELECT LibroActasid, LibroNumero, FolioNumero, FolioMaximo, FechaAlta, FechaBaja, Activo, TipoLibroId FROM LibroActas";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new LibroActasModelo
                    {
                        LibroActaId = reader.GetInt32(0),
                        LibroNumero = reader.GetInt32(1),
                        FolioNumero = reader.GetInt32(2),
                        FolioMaximo = reader.GetInt32(3),
                        FechaAlta = reader.GetDateTime(4),
                        FechaBaja = reader.GetDateTime(5),
                        Activo = reader.GetBoolean(6),
                        TipoLibroId = reader.GetInt32(7)
                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<LibroActasModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT LibroActasid, LibroNumero, FolioNumero, FolioMaximo, FechaAlta, FechaBaja, Activo, TipoLibroId FROM LibroActas WHERE LibroActasid = @LibroActasid";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@LibroActasid", id);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new LibroActasModelo
                    {
                        LibroActaId = reader.GetInt32(0),
                        LibroNumero = reader.GetInt32(1),
                        FolioNumero = reader.GetInt32(2),
                        FolioMaximo = reader.GetInt32(3),
                        FechaAlta = reader.GetDateTime(4),
                        FechaBaja = reader.GetDateTime(5),
                        Activo = reader.GetBoolean(6),
                        TipoLibroId = reader.GetInt32(7)
                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<int> CreateAsync(LibroActasModelo libroActas, CancellationToken cancellationToken = default)
        {
            if (libroActas == null) throw new ArgumentNullException(nameof(libroActas));
            const string sql = "INSERT INTO LibroActas (LibroNumero, FolioNumero, FolioMaximo, FechaAlta, FechaBaja, Activo, TipoLibroId) VALUES (@LibroNumero, @FolioNumero, @FolioMaximo, @FechaAlta, @FechaBaja, @Activo, @TipoLibroId); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@LibroNumero", libroActas.LibroNumero);
                cmd.Parameters.AddWithValue("@FolioNumero", libroActas.FolioNumero);
                cmd.Parameters.AddWithValue("@FolioMaximo", libroActas.FolioMaximo);
                cmd.Parameters.AddWithValue("@FechaAlta", libroActas.FechaAlta);
                cmd.Parameters.AddWithValue("@FechaBaja", libroActas.FechaBaja);
                cmd.Parameters.AddWithValue("@Activo", libroActas.Activo);
                cmd.Parameters.AddWithValue("@TipoLibroId", libroActas.TipoLibroId);
                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(LibroActasModelo libroActas, CancellationToken cancellationToken = default)
        {
            if (libroActas == null) throw new ArgumentNullException(nameof(libroActas));
            const string sql = "UPDATE LibroActas SET LibroNumero = @LibroNumero, FolioNumero = @FolioNumero, FolioMaximo = @FolioMaximo, FechaAlta = @FechaAlta, FechaBaja = @FechaBaja, Activo = @Activo, TipoLibroId = @TipoLibroId WHERE LibroActasid = @LibroActasid";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@LibroNumero", libroActas.LibroNumero);
                cmd.Parameters.AddWithValue("@FolioNumero", libroActas.FolioNumero);
                cmd.Parameters.AddWithValue("@FolioMaximo", libroActas.FolioMaximo);
                cmd.Parameters.AddWithValue("@FechaAlta", libroActas.FechaAlta);
                cmd.Parameters.AddWithValue("@FechaBaja", libroActas.FechaBaja);
                cmd.Parameters.AddWithValue("@Activo", libroActas.Activo);
                cmd.Parameters.AddWithValue("@TipoLibroId", libroActas.TipoLibroId);
                cmd.Parameters.AddWithValue("@LibroActasid", libroActas.LibroActaId);
                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> DeleteAsync(int LibrosActasid, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM LibroActas WHERE LibroActasid = @LibroActasid";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@LibroActasid", LibrosActasid);
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