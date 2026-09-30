using System.Data;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using Microsoft.Data.SqlClient;

namespace instituto93.Data.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private const int PasswordWorkFactor = 12;

    private const string SelectUsuarioConAlumno = """
        SELECT u.Id, u.AlumnoId, u.Password, u.Activo,
               a.Nombre, a.Apellido, a.NumeroDocumento, a.Email, a.Activo
        FROM Usuarios u
        JOIN Alumnos a ON a.AlumnoId = u.AlumnoId
        """;

    private readonly Conexion _conexion;

    public UsuarioRepository(Conexion conexion)
    {
        _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
    }

    public Task<Usuario?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return GetSingleAsync(
            $"{SelectUsuarioConAlumno} WHERE u.Id = @id;",
            command => command.Parameters.Add("@id", SqlDbType.Int).Value = id,
            cancellationToken);
    }

    public Task<Usuario?> GetByAlumnoIdAsync(int alumnoId, CancellationToken cancellationToken = default)
    {
        return GetSingleAsync(
            $"{SelectUsuarioConAlumno} WHERE u.AlumnoId = @alumnoId;",
            command => command.Parameters.Add("@alumnoId", SqlDbType.Int).Value = alumnoId,
            cancellationToken);
    }

    public Task<Usuario?> GetByDniAsync(string dni, CancellationToken cancellationToken = default)
    {
        return GetSingleAsync(
            $"{SelectUsuarioConAlumno} WHERE {AlumnoRepository.NumeroDocumentoNormalizadoSql("a")} = @dni;",
            command => command.Parameters.Add("@dni", SqlDbType.VarChar, 30).Value = dni,
            cancellationToken);
    }

    public async Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        if (string.IsNullOrWhiteSpace(usuario.Password))
            throw new ArgumentException("La contraseña es obligatoria.", nameof(usuario));

        const string sql = """
            INSERT INTO Usuarios (AlumnoId, Password, Activo)
            VALUES (@alumnoId, @password, @activo);
            SELECT CAST(SCOPE_IDENTITY() AS int);
            """;

        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("@alumnoId", SqlDbType.Int).Value = usuario.AlumnoId;
            command.Parameters.Add("@password", SqlDbType.NVarChar, 512).Value = HashPassword(usuario.Password);
            command.Parameters.Add("@activo", SqlDbType.Bit).Value = usuario.Activo;

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

    private async Task<Usuario?> GetSingleAsync(
        string sql,
        Action<SqlCommand> addParameters,
        CancellationToken cancellationToken)
    {
        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = sql;
            addParameters(command);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
                return null;

            return new Usuario
            {
                Id = reader.GetInt32(0),
                AlumnoId = reader.GetInt32(1),
                Password = reader.GetString(2),
                Activo = reader.GetBoolean(3),
                Alumno = new AlumnoModelo
                {
                    AlumnoId = reader.GetInt32(1),
                    Nombre = reader.GetString(4),
                    Apellido = reader.GetString(5),
                    NumeroDocumento = reader.GetString(6),
                    Email = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Activo = reader.IsDBNull(8) ? null : reader.GetBoolean(8)
                }
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
