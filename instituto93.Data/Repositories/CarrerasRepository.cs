using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Interfaces;
using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories
{
    public class CarrerasRepository : ICarrerasRepository
    {
        private readonly Conexion _conexion;
        public CarrerasRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }
        public async Task<IEnumerable<CarrerasModelo>> GetAllAsync(CancellationToken cancellationToken = default)
        { 
            var lista = new List<CarrerasModelo>();
            const string sql = "SELECT CarreraId, Titulo, Nombre, DescripcionCorta, JefeCatedra, AnioInicio, AnioFin, Activo, PlanEstudio, Resolucion, "
                    + "Correlatividades, ImagenDescriptiva, NumeroExpediente, CantidadHoras, Duracion, CarreraEstadoId, CarrerasCodigoBloque FROM Carreras";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new CarrerasModelo
                    {
                        CarreraId = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        Nombre = reader.GetString(2),
                        DescripcionCorta = reader.GetString(3),
                        JefeCatedra = reader.GetString(4),
                        AnioInicio = reader.GetInt32(5),
                        AnioFin = reader.GetInt32(6),
                        Activo = reader.GetBoolean(7),
                        PlanEstudio = reader.GetString(8),
                        Resolucion = reader.GetString(9),
                        Correlatividades = reader.GetString(10),
                        ImagenDescriptiva = reader.GetString(11),
                        NumeroExpediente = reader.GetString(12),
                        CantidadHoras = reader.GetInt32(13),
                        Duracion = reader.GetInt32(14),
                        CarreraEstadoId = reader.GetInt32(15),
                        CarrerasCodigoBloque = reader.GetString(16),
                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }
        public async Task<CarrerasModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT CarreraId, Titulo, Nombre, DescripcionCorta, JefeCatedra, AnioInicio, AnioFin, Activo, PlanEstudio, Resolucion, Correlatividades, "
                            + "ImagenDescriptiva, NumeroExpediente, CantidadHoras, Duracion, CarreraEstadoId, CarrerasCodigoBloque FROM Carreras WHERE CarreraId = @CarreraId";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@CarreraId", id);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new CarrerasModelo
                    {
                        CarreraId = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        Nombre = reader.GetString(2),
                        DescripcionCorta = reader.GetString(3),
                        JefeCatedra = reader.GetString(4),
                        AnioInicio = reader.GetInt32(5),
                        AnioFin = reader.GetInt32(6),
                        Activo = reader.GetBoolean(7),
                        PlanEstudio = reader.GetString(8),
                        Resolucion = reader.GetString(9),
                        Correlatividades = reader.GetString(10),
                        ImagenDescriptiva = reader.GetString(11),
                        NumeroExpediente = reader.GetString(12),
                        CantidadHoras = reader.GetInt32(13),
                        Duracion = reader.GetInt32(14),
                        CarreraEstadoId = reader.GetInt32(15),
                        CarrerasCodigoBloque = reader.GetString(16)
                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }
        public async Task<int> CreateAsync(CarrerasModelo carreras, CancellationToken cancellationToken = default)
        {
            if (carreras == null) throw new ArgumentNullException(nameof(carreras));
            const string sql = "INSERT INTO Carreras (Titulo, Nombre, DescripcionCorta, JefeCatedra, AnioInicio, AnioFin, Activo, PlanEstudio, Resolucion, Correlatividades, " +
                               "ImagenDescriptiva, NumeroExpediente, CantidadHoras, Duracion, CarreraEstadoId, CarrerasCodigoBloque)" +
                               "VALUES (@Titulo, @Nombre, @DescripcionCorta, @JefeCatedra, @AnioInicio, @AnioFin, @Activo, @PlanEstudio, @Resolucion, @Correlatividades, " +
                               "@ImagenDescriptiva, @NumeroExpediente, @CantidadHoras, @Duracion, @CarreraEstadoId, @CarrerasCodigoBloque); SELECT CAST(SCOPE_IDENTITY() AS INT)";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Titulo", carreras.Titulo);
                cmd.Parameters.AddWithValue("@Nombre", carreras.Nombre);
                cmd.Parameters.AddWithValue("@DescripcionCorta", carreras.DescripcionCorta);
                cmd.Parameters.AddWithValue("@JefeCatedra", carreras.JefeCatedra);
                cmd.Parameters.AddWithValue("@AnioInicio", carreras.AnioInicio);
                cmd.Parameters.AddWithValue("@AnioFin", carreras.AnioFin);
                cmd.Parameters.AddWithValue("@Activo", carreras.Activo);
                cmd.Parameters.AddWithValue("@PlanEstudio", carreras.PlanEstudio);
                cmd.Parameters.AddWithValue("@Resolucion", carreras.Resolucion);
                cmd.Parameters.AddWithValue("@Correlatividades", carreras.Correlatividades);
                cmd.Parameters.AddWithValue("@ImagenDescriptiva", carreras.ImagenDescriptiva);
                cmd.Parameters.AddWithValue("@NumeroExpediente", carreras.NumeroExpediente);
                cmd.Parameters.AddWithValue("@CantidadHoras", carreras.CantidadHoras);
                cmd.Parameters.AddWithValue("@Duracion", carreras.Duracion);
                cmd.Parameters.AddWithValue("@CarreraEstadoId", carreras.CarreraEstadoId);
                cmd.Parameters.AddWithValue("@CarrerasCodigoBloque", carreras.CarrerasCodigoBloque);
                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }
        public async Task<bool> UpdateAsync(CarrerasModelo carreras, CancellationToken cancellationToken = default)
        {
            if (carreras == null) throw new ArgumentNullException(nameof(carreras));
            const string sql = "UPDATE Carreras SET Titulo = @Titulo, Nombre = @Nombre, DescripcionCorta = @DescripcionCorta, JefeCatedra = @JefeCatedra, " +
                             "AnioInicio = @AnioInicio, AnioFin = @AnioFin, Activo = @Activo, PlanEstudio = @PlanEstudio, Resolucion = @Resolucion, " +
                             "Correlatividades = @Correlatividades, ImagenDescriptiva = @ImagenDescriptiva, NumeroExpediente = @NumeroExpediente, " +
                             "CantidadHoras = @CantidadHoras, Duracion = @Duracion, CarreraEstadoId = @CarreraEstadoId, CarrerasCodigoBloque = @CarrerasCodigoBloque WHERE CarreraId = @CarreraId";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@CarreraId", carreras.CarreraId);
                cmd.Parameters.AddWithValue("@Titulo", carreras.Titulo);
                cmd.Parameters.AddWithValue("@Nombre", carreras.Nombre);
                cmd.Parameters.AddWithValue("@DescripcionCorta", carreras.DescripcionCorta);
                cmd.Parameters.AddWithValue("@JefeCatedra", carreras.JefeCatedra);
                cmd.Parameters.AddWithValue("@AnioInicio", carreras.AnioInicio);
                cmd.Parameters.AddWithValue("@AnioFin", carreras.AnioFin);
                cmd.Parameters.AddWithValue("@Activo", carreras.Activo);
                cmd.Parameters.AddWithValue("@PlanEstudio", carreras.PlanEstudio);
                cmd.Parameters.AddWithValue("@Resolucion", carreras.Resolucion);
                cmd.Parameters.AddWithValue("@Correlatividades", carreras.Correlatividades);
                cmd.Parameters.AddWithValue("@ImagenDescriptiva", carreras.ImagenDescriptiva);
                cmd.Parameters.AddWithValue("@NumeroExpediente", carreras.NumeroExpediente);
                cmd.Parameters.AddWithValue("@CantidadHoras", carreras.CantidadHoras);
                cmd.Parameters.AddWithValue("@Duracion", carreras.Duracion);
                cmd.Parameters.AddWithValue("@CarreraEstadoId", carreras.CarreraEstadoId);
                cmd.Parameters.AddWithValue("@CarrerasCodigoBloque", carreras.CarrerasCodigoBloque);
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
            const string sql = "DELETE FROM Carreras WHERE CarreraId = @CarreraId";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@CarreraId", id);
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
