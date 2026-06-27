using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using instituto93.Domain.Models;
using instituto93.Data.Repositories.Interfaces;

namespace instituto93.Data.Repositories
{
    public class PersonalRepository : IPersonalRepository
    {
        private readonly Conexion _conexion;

        public PersonalRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<PersonalModelo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<PersonalModelo>();
            const string sql = "SELECT PersonalId, Nombre, Apellido, NumeroDocumento FROM PersonalModelo";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new PersonalModelo
                    {
                        PersonalId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        NumeroDocumento = reader.GetString(3)

                    });
                }
            }
            finally
            {
                _conexion.Close();
            }

            return lista;
        }

        public async Task<PersonalModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT PersonalId, Nombre, Apellido, NumeroDocumento FROM Personal WHERE PersonalId = @id";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new PersonalModelo
                    {
                        PersonalId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        NumeroDocumento = reader.GetString(3)
                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }
        // NumeroDocumento, Nombre, Apellido, FechaNacimiento, Sexo, Titulo, TramoPedagogico -- obligatorios
        public async Task<int> CreateAsync(PersonalModelo personal, CancellationToken cancellationToken = default)
        {
            if (personal == null) throw new ArgumentNullException(nameof(personal));
            // const string sql = "INSERT INTO Personal (descripcion) VALUES (@descripcion); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            const string sql = "INSERT INTO Personal(NumeroDocumento, Nombre, Apellido, FechaNacimiento, Sexo, Titulo, TramoPedagogico)  VALUES (@NumeroDocumento, @Nombre, @Apellido, @FechaNacimiento, @Sexo, @Titulo, @TramoPedagogico) ";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@NumeroDocumento", personal.NumeroDocumento ?? string.Empty);
                cmd.Parameters.AddWithValue("@Nombre", personal.Nombre ?? string.Empty);
                cmd.Parameters.AddWithValue("@Apellido", personal.Apellido ?? string.Empty);
                cmd.Parameters.AddWithValue("@FechaNacimiento", personal.FechaNacimiento ?? DateTime.Now());
                cmd.Parameters.AddWithValue("@Sexo", personal.Sexo ?? string.Empty);
                cmd.Parameters.AddWithValue("@Titulo", personal.Titulo ?? string.Empty);
                cmd.Parameters.AddWithValue("@TramoPedagogico", personal.TramoPedagogico ?? string.Empty);
                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> UpdateAsync(PersonalModelo personal, CancellationToken cancellationToken = default)
        {
            if (PersonalModelo == null) throw new ArgumentNullException(nameof(PersonalModelo));
            const string sql = "UPDATE Personal SET NumeroDocumento = @NumeroDocumento, Nombre = @Nombre, Apellido = @Apellido, FechaNacimiento = @FechaNacimiento, Sexo = @Sexo, Titulo = @Titulo, TramoPedagogico = @TramoPedagogico WHERE PersonalId = @id";
            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@NumeroDocumento", personal.NumeroDocumento ?? string.Empty);
                cmd.Parameters.AddWithValue("@Nombre", personal.Nombre ?? string.Empty);
                cmd.Parameters.AddWithValue("@Apellido", personal.Apellido ?? string.Empty);
                cmd.Parameters.AddWithValue("@FechaNacimiento", personal.FechaNacimiento ?? DateTime.Now());
                cmd.Parameters.AddWithValue("@Sexo", personal.Sexo ?? string.Empty);
                cmd.Parameters.AddWithValue("@Titulo", personal.Titulo ?? string.Empty);
                cmd.Parameters.AddWithValue("@TramoPedagogico", personal.TramoPedagogico ?? string.Empty);
                cmd.Parameters.AddWithValue("@id", id);
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
            const string sql = "DELETE FROM Personal WHERE PersonalId = @id";
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