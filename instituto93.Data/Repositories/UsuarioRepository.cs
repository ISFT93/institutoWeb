using System.Data;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using Microsoft.Data.SqlClient;

namespace instituto93.Data.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private const int PasswordWorkFactor = 12;

    private const string SelectUsuario = """
        SELECT u.Id, u.RolId, u.AlumnoId, u.ProfesorId, u.Password, u.Activo, r.Nombre,
               a.Nombre, a.Apellido, a.NumeroDocumento, a.Email, a.Activo,
               p.Nombre, p.Apellido, p.NumeroDocumento, p.Email,
               CASE WHEN p.FechaBaja IS NULL THEN 1 ELSE 0 END
        FROM Usuarios u
        JOIN Roles r ON r.Id = u.RolId
        LEFT JOIN Alumnos a ON a.AlumnoId = u.AlumnoId
        LEFT JOIN Personal p ON p.PersonalId = u.ProfesorId
        """;

    private readonly Conexion _conexion;

    public UsuarioRepository(Conexion conexion)
    {
        _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
    }

    public Task<Usuario?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return GetSingleAsync(
            $"{SelectUsuario} WHERE u.Id = @id;",
            command => command.Parameters.Add("@id", SqlDbType.Int).Value = id,
            cancellationToken);
    }

    public Task<Usuario?> GetByAlumnoIdAsync(int alumnoId, CancellationToken cancellationToken = default)
    {
        return GetSingleAsync(
            $"{SelectUsuario} WHERE u.AlumnoId = @alumnoId;",
            command => command.Parameters.Add("@alumnoId", SqlDbType.Int).Value = alumnoId,
            cancellationToken);
    }

    public Task<Usuario?> GetByProfesorIdAsync(int profesorId, CancellationToken cancellationToken = default)
    {
        return GetSingleAsync(
            $"{SelectUsuario} WHERE u.ProfesorId = @profesorId;",
            command => command.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId,
            cancellationToken);
    }

    public Task<Usuario?> GetByDniAsync(string dni, CancellationToken cancellationToken = default)
    {
        return GetSingleAsync(
            $"""
            {SelectUsuario}
            WHERE {AlumnoRepository.NumeroDocumentoNormalizadoSql("a")} = @dni
               OR {AlumnoRepository.NumeroDocumentoNormalizadoSql("p")} = @dni
            ORDER BY CASE WHEN u.ProfesorId IS NOT NULL THEN 0 ELSE 1 END;
            """,
            command => command.Parameters.Add("@dni", SqlDbType.VarChar, 30).Value = dni,
            cancellationToken);
    }

    public async Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        if (string.IsNullOrWhiteSpace(usuario.Password))
            throw new ArgumentException("La contraseña es obligatoria.", nameof(usuario));

        const string sql = """
            INSERT INTO Usuarios (RolId, AlumnoId, ProfesorId, Password, Activo)
            VALUES (
                (SELECT Id FROM Roles WHERE Nombre = @rol),
                @alumnoId, @profesorId, @password, @activo);
            SELECT CAST(SCOPE_IDENTITY() AS int);
            """;

        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("@rol", SqlDbType.NVarChar, 50).Value =
                usuario.ProfesorId is not null ? Rol.Docente : Rol.Alumno;
            command.Parameters.Add("@alumnoId", SqlDbType.Int).Value = (object?)usuario.AlumnoId ?? DBNull.Value;
            command.Parameters.Add("@profesorId", SqlDbType.Int).Value = (object?)usuario.ProfesorId ?? DBNull.Value;
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

            string Text(int ordinal) => reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);

            var usuario = new Usuario
            {
                Id = reader.GetInt32(0),
                RolId = reader.GetInt32(1),
                AlumnoId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                ProfesorId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                Password = reader.GetString(4),
                Activo = reader.GetBoolean(5),
                Rol = new Rol { Id = reader.GetInt32(1), Nombre = reader.GetString(6) }
            };

            if (usuario.AlumnoId is int alumnoId)
            {
                usuario.Alumno = new AlumnoModelo
                {
                    AlumnoId = alumnoId,
                    Nombre = Text(7),
                    Apellido = Text(8),
                    NumeroDocumento = Text(9),
                    Email = Text(10),
                    Activo = reader.IsDBNull(11) ? null : reader.GetBoolean(11)
                };
            }

            if (usuario.ProfesorId is int profesorId)
            {
                usuario.Profesor = new Profesores
                {
                    ProfesorId = profesorId,
                    Nombre = Text(12),
                    Apellido = Text(13),
                    NumeroDocumento = Text(14),
                    Email = Text(15),
                    Activo = (byte)reader.GetInt32(16)
                };
            }

            return usuario;
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
