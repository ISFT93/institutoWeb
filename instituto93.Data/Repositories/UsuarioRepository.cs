using System.Data;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using Microsoft.Data.SqlClient;

namespace instituto93.Data.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private const int PasswordWorkFactor = 12;

    private readonly Conexion _conexion;

    public UsuarioRepository(Conexion conexion)
    {
        _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
    }

    public Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, Nombre, Apellido, FechaNacimiento, Email, Password, Dni, Telefono, Direccion, LocalidadId, Activo
            FROM Usuarios
            WHERE Email = @email;
            """;

        return GetByIdentifierAsync(sql, "@email", email, cancellationToken);
    }

    public Task<Usuario?> GetByEmailOrDniAsync(string emailOrDni, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, Nombre, Apellido, FechaNacimiento, Email, Password, Dni, Telefono, Direccion, LocalidadId, Activo
            FROM Usuarios
            WHERE Email = @identifier OR Dni = @identifier;
            """;

        return GetByIdentifierAsync(sql, "@identifier", emailOrDni, cancellationToken);
    }

    public async Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        if (string.IsNullOrWhiteSpace(usuario.Password))
            throw new ArgumentException("La contraseña es obligatoria.", nameof(usuario));

        const string sql = """
            INSERT INTO Usuarios (Nombre, Apellido, FechaNacimiento, Email, Password, Dni, Telefono, Direccion, LocalidadId, Activo)
            VALUES (@nombre, @apellido, @fechaNacimiento, @email, @password, @dni, @telefono, @direccion, @localidadId, @activo);
            SELECT CAST(SCOPE_IDENTITY() AS int);
            """;

        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = usuario.Nombre;
            command.Parameters.Add("@apellido", SqlDbType.NVarChar, 100).Value = usuario.Apellido;
            command.Parameters.Add("@fechaNacimiento", SqlDbType.DateTime2).Value = usuario.FechaNacimiento;
            command.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = usuario.Email;
            command.Parameters.Add("@password", SqlDbType.NVarChar, 512).Value = HashPassword(usuario.Password);
            command.Parameters.Add("@dni", SqlDbType.NVarChar, 30).Value = usuario.Dni;
            command.Parameters.Add("@telefono", SqlDbType.NVarChar, 50).Value = usuario.Telefono;
            command.Parameters.Add("@direccion", SqlDbType.NVarChar, 256).Value = usuario.Direccion;
            command.Parameters.Add("@localidadId", SqlDbType.Int).Value = usuario.LocalidadId;
            command.Parameters.Add("@activo", SqlDbType.Bit).Value = usuario.activo;

            usuario.Id = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
        }
        finally
        {
            _conexion.Close();
        }
    }

    public bool VerifyPassword(string storedHash, string password)
    {
        if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(password))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            return false;
        }
    }

    private async Task<Usuario?> GetByIdentifierAsync(
        string sql,
        string parameterName,
        string value,
        CancellationToken cancellationToken)
    {
        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add(parameterName, SqlDbType.NVarChar, 256).Value = value;
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
                return null;

            return new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                Apellido = reader.GetString(2),
                FechaNacimiento = reader.GetDateTime(3),
                Email = reader.GetString(4),
                Password = reader.GetString(5),
                Dni = reader.GetString(6),
                Telefono = reader.GetString(7),
                Direccion = reader.GetString(8),
                LocalidadId = reader.GetInt32(9),
                activo = reader.GetBoolean(10)
            };
        }
        finally
        {
            _conexion.Close();
        }
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, PasswordWorkFactor);
    }
}
