using instituto93.Domain.Models;
using instituto93.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories
{
    public class MateriasModeloRepository: IMateriasModeloRepository
    {
        private readonly Conexion _conexion;

        public MateriasModeloRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<MateriasModelo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<MateriasModelo>();
            const string sql = "SELECT MateriaId, Nombre, AnioCarreraId, EspacioId, CargaHoraria, Modulos, Activo, CarreraId, AnioCarrera FROM MateriasModelo";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new MateriasModelo
                    {
                        MateriaId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        AnioCarreraId = reader.GetInt32(2),
                        EspacioId = reader.GetInt32(3),
                        CargaHoraria = reader.GetInt32(4),
                        Modulos = reader.GetInt32(5),
                        Activo = reader.GetBoolean(6),
                        CarreraId = reader.GetInt32(7),
                        AnioCarrera = reader.GetInt32(8)
                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<MateriasModelo?> GetByIdAsync(int MateriaId, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT MateriaId, Nombre, AnioCarreraId, EspacioId, CargaHoraria, Modulos, Activo, CarreraId, AnioCarrera FROM MateriasModelo WHERE MateriaId = @MateriaId";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@MateriaId", MateriaId);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new MateriasModelo
                    {
                        MateriaId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        AnioCarreraId = reader.GetInt32(2),
                        EspacioId = reader.GetInt32(3),
                        CargaHoraria = reader.GetInt32(4),
                        Modulos = reader.GetInt32(5),
                        Activo = reader.GetBoolean(6),
                        CarreraId = reader.GetInt32(7),
                        AnioCarrera = reader.GetInt32(8)
                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<int> CreateAsync(MateriasModelo materia, CancellationToken cancellationToken = default)
        {
            if (materia == null) throw new ArgumentNullException(nameof(materia));
            const string sql = "INSERT INTO MateriasModelo (Nombre, AnioCarreraId, EspacioId, CargaHoraria, Modulos, Activo, CarreraId, AnioCarrera) VALUES (@Nombre, @AnioCarreraId, @EspacioId, @CargaHoraria, @Modulos, @Activo, @CarreraId, @AnioCarrera); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Nombre", materia.Nombre ?? string.Empty);
                cmd.Parameters.AddWithValue("@AnioCarreraId", materia.AnioCarreraId);
                cmd.Parameters.AddWithValue("@EspacioId", materia.EspacioId);
                cmd.Parameters.AddWithValue("@CargaHoraria", materia.CargaHoraria);
                cmd.Parameters.AddWithValue("@Modulos", materia.Modulos);
                cmd.Parameters.AddWithValue("@Activo", materia.Activo);
                cmd.Parameters.AddWithValue("@CarreraId", materia.CarreraId);
                cmd.Parameters.AddWithValue("@AnioCarrera", materia.AnioCarrera);
                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(MateriasModelo materia, CancellationToken cancellationToken = default)
        {
            if (materia == null) throw new ArgumentNullException(nameof(materia));
            const string sql = "UPDATE MateriasModelo SET Nombre = @Nombre, AnioCarreraId = @AnioCarreraId, EspacioId = @EspacioId, CargaHoraria = @CargaHoraria, Modulos = @Modulos, Activo = @Activo, CarreraId = @CarreraId, AnioCarrera = @AnioCarrera WHERE MateriaId = @MateriaId";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Nombre", materia.Nombre ?? string.Empty);
                cmd.Parameters.AddWithValue("@AnioCarreraId", materia.AnioCarreraId);
                cmd.Parameters.AddWithValue("@EspacioId", materia.EspacioId);
                cmd.Parameters.AddWithValue("@CargaHoraria", materia.CargaHoraria);
                cmd.Parameters.AddWithValue("@Modulos", materia.Modulos);
                cmd.Parameters.AddWithValue("@Activo", materia.Activo);
                cmd.Parameters.AddWithValue("@CarreraId", materia.CarreraId);
                cmd.Parameters.AddWithValue("@AnioCarrera", materia.AnioCarrera);
                cmd.Parameters.AddWithValue("@MateriaId", materia.MateriaId);
                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> DeleteAsync(int MateriaId, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM MateriasModelo WHERE MateriaId = @MateriaId";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@MateriaId", MateriaId);
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
