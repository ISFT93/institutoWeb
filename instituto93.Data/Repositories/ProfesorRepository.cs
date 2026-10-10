using System.Data;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories;

// Los docentes viven en la tabla Personal; "activo" significa FechaBaja nula.
public sealed class ProfesorRepository : IProfesorRepository
{
    private readonly Conexion _conexion;

    public ProfesorRepository(Conexion conexion)
    {
        _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
    }

    public async Task<Profesores?> GetByDocumentoAsync(string numeroDocumento, CancellationToken cancellationToken = default)
    {
        var sql = $"""
            SELECT TOP 1 p.PersonalId, p.Apellido, p.Nombre, p.NumeroDocumento, p.Email,
                   CASE WHEN p.FechaBaja IS NULL THEN 1 ELSE 0 END
            FROM Personal p
            WHERE {AlumnoRepository.NumeroDocumentoNormalizadoSql("p")} = @numeroDocumento
            ORDER BY p.PersonalId;
            """;

        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("@numeroDocumento", SqlDbType.VarChar, 30).Value = numeroDocumento;
            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken))
                return null;

            string Text(int ordinal) => reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);

            return new Profesores
            {
                ProfesorId = reader.GetInt32(0),
                Apellido = Text(1),
                Nombre = Text(2),
                NumeroDocumento = Text(3),
                Email = Text(4),
                Activo = (byte)reader.GetInt32(5)
            };
        }
        finally
        {
            _conexion.Close();
        }
    }

    public async Task<int> CreateAsync(Profesores profesor, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(profesor);

        const string sql = """
            INSERT INTO Personal (NumeroDocumento, Nombre, Apellido, FechaNacimiento, Sexo, Direccion,
                                  Localidad, Nacionalidad, Email, FechaAlta)
            VALUES (@numeroDocumento, @nombre, @apellido, @fechaNacimiento, @sexo, @direccion,
                    @localidad, @nacionalidad, @email, CAST(GETDATE() AS date));
            SELECT CAST(SCOPE_IDENTITY() AS int);
            """;

        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("@numeroDocumento", SqlDbType.VarChar, 30).Value = profesor.NumeroDocumento;
            command.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = profesor.Nombre;
            command.Parameters.Add("@apellido", SqlDbType.VarChar, 100).Value = profesor.Apellido;
            command.Parameters.Add("@fechaNacimiento", SqlDbType.Date).Value =
                DateTime.TryParse(profesor.FechaNacimiento, out var fecha) ? fecha : DBNull.Value;
            command.Parameters.Add("@sexo", SqlDbType.Char, 1).Value = profesor.Sexo;
            command.Parameters.Add("@direccion", SqlDbType.VarChar, 200).Value = profesor.Direccion;
            command.Parameters.Add("@localidad", SqlDbType.VarChar, 100).Value = profesor.Localidad;
            command.Parameters.Add("@nacionalidad", SqlDbType.VarChar, 100).Value = profesor.Nacionalidad;
            command.Parameters.Add("@email", SqlDbType.VarChar, 200).Value = profesor.Email;

            return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
        }
        finally
        {
            _conexion.Close();
        }
    }
}
