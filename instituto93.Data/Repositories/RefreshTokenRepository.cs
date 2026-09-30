using System.Data;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using Microsoft.Data.SqlClient;

namespace instituto93.Data.Repositories;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private const string InsertSql = """
        INSERT INTO RefreshTokens (UsuarioId, FamilyId, TokenHash, CreatedAt, ExpiresAt, FamilyExpiresAt)
        VALUES (@usuarioId, @familyId, @tokenHash, @createdAt, @expiresAt, @familyExpiresAt);
        SELECT CAST(SCOPE_IDENTITY() AS int);
        """;

    private readonly Conexion _conexion;

    public RefreshTokenRepository(Conexion conexion)
    {
        _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
    }

    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(token);

        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = InsertSql;
            AddInsertParameters(command, token);
            token.Id = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
        }
        finally
        {
            _conexion.Close();
        }
    }

    public async Task<RefreshToken?> GetByHashAsync(byte[] tokenHash, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, UsuarioId, FamilyId, TokenHash, CreatedAt, ExpiresAt, FamilyExpiresAt, UsedAt, RevokedAt, ReplacedById
            FROM RefreshTokens
            WHERE TokenHash = @tokenHash;
            """;

        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("@tokenHash", SqlDbType.VarBinary, 32).Value = tokenHash;
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
                return null;

            return new RefreshToken
            {
                Id = reader.GetInt32(0),
                UsuarioId = reader.GetInt32(1),
                FamilyId = reader.GetGuid(2),
                TokenHash = (byte[])reader[3],
                CreatedAt = AsUtc(reader.GetDateTime(4)),
                ExpiresAt = AsUtc(reader.GetDateTime(5)),
                FamilyExpiresAt = AsUtc(reader.GetDateTime(6)),
                UsedAt = reader.IsDBNull(7) ? null : AsUtc(reader.GetDateTime(7)),
                RevokedAt = reader.IsDBNull(8) ? null : AsUtc(reader.GetDateTime(8)),
                ReplacedById = reader.IsDBNull(9) ? null : reader.GetInt32(9)
            };
        }
        finally
        {
            _conexion.Close();
        }
    }

    public async Task<bool> RotateAsync(int currentId, RefreshToken replacement, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(replacement);

        const string markUsedSql = """
            UPDATE RefreshTokens
            SET UsedAt = @usedAt
            WHERE Id = @id AND UsedAt IS NULL AND RevokedAt IS NULL;
            """;

        const string linkSql = "UPDATE RefreshTokens SET ReplacedById = @replacedById WHERE Id = @id;";

        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var transaction = (SqlTransaction)await _conexion.Conector.BeginTransactionAsync(cancellationToken);

            using (var markUsed = _conexion.Conector.CreateCommand())
            {
                markUsed.Transaction = transaction;
                markUsed.CommandText = markUsedSql;
                markUsed.Parameters.Add("@id", SqlDbType.Int).Value = currentId;
                markUsed.Parameters.Add("@usedAt", SqlDbType.DateTime2).Value = replacement.CreatedAt;

                if (await markUsed.ExecuteNonQueryAsync(cancellationToken) == 0)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return false;
                }
            }

            using (var insert = _conexion.Conector.CreateCommand())
            {
                insert.Transaction = transaction;
                insert.CommandText = InsertSql;
                AddInsertParameters(insert, replacement);
                replacement.Id = Convert.ToInt32(await insert.ExecuteScalarAsync(cancellationToken));
            }

            using (var link = _conexion.Conector.CreateCommand())
            {
                link.Transaction = transaction;
                link.CommandText = linkSql;
                link.Parameters.Add("@id", SqlDbType.Int).Value = currentId;
                link.Parameters.Add("@replacedById", SqlDbType.Int).Value = replacement.Id;
                await link.ExecuteNonQueryAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            return true;
        }
        finally
        {
            _conexion.Close();
        }
    }

    public async Task RevokeFamilyAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE RefreshTokens
            SET RevokedAt = SYSUTCDATETIME()
            WHERE FamilyId = @familyId AND RevokedAt IS NULL;
            """;

        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("@familyId", SqlDbType.UniqueIdentifier).Value = familyId;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        finally
        {
            _conexion.Close();
        }
    }

    public async Task DeleteInactiveFamiliesAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            DELETE FROM RefreshTokens
            WHERE UsuarioId = @usuarioId
              AND FamilyId IN (
                  SELECT FamilyId
                  FROM RefreshTokens
                  WHERE UsuarioId = @usuarioId
                  GROUP BY FamilyId
                  HAVING MAX(CASE
                      WHEN RevokedAt IS NULL AND UsedAt IS NULL AND ExpiresAt > SYSUTCDATETIME() THEN 1
                      ELSE 0
                  END) = 0);
            """;

        await _conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = _conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("@usuarioId", SqlDbType.Int).Value = usuarioId;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        finally
        {
            _conexion.Close();
        }
    }

    private static void AddInsertParameters(SqlCommand command, RefreshToken token)
    {
        command.Parameters.Add("@usuarioId", SqlDbType.Int).Value = token.UsuarioId;
        command.Parameters.Add("@familyId", SqlDbType.UniqueIdentifier).Value = token.FamilyId;
        command.Parameters.Add("@tokenHash", SqlDbType.VarBinary, 32).Value = token.TokenHash;
        command.Parameters.Add("@createdAt", SqlDbType.DateTime2).Value = token.CreatedAt;
        command.Parameters.Add("@expiresAt", SqlDbType.DateTime2).Value = token.ExpiresAt;
        command.Parameters.Add("@familyExpiresAt", SqlDbType.DateTime2).Value = token.FamilyExpiresAt;
    }

    private static DateTime AsUtc(DateTime value) => DateTime.SpecifyKind(value, DateTimeKind.Utc);
}
