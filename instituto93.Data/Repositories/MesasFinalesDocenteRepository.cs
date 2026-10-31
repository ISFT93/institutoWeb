using System.Data;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.DTOs;
using instituto93.Domain.Rules;
using Microsoft.Data.SqlClient;

namespace instituto93.Data.Repositories;

public sealed class MesasFinalesDocenteRepository(Conexion conexion) : IMesasFinalesDocenteRepository
{
    public async Task<List<MesaFinalDocenteDto>> GetByProfesorAsync(
        int profesorId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT mf.MesaFinalId,
                   COALESCE(m.Nombre, N'Materia sin nombre'),
                   COALESCE(c.Nombre, N'Carrera sin nombre'),
                   mf.Fecha,
                   COALESCE(t.Descripcion, N'Sin turno'),
                   COALESCE(e.Descripcion, N'Sin estado'),
                   mf.FinalEstadoId,
                   COUNT(ma.MesaAlumnoId)
            FROM MesasFinales mf
            LEFT JOIN Materias m ON m.MateriaId = mf.MateriaId
            LEFT JOIN Carreras c ON c.CarreraId = mf.CarreraId
            LEFT JOIN Turnos t ON t.TurnoId = mf.TurnoId
            LEFT JOIN Estados e ON e.EstadoId = mf.FinalEstadoId
            LEFT JOIN MesaAlumno ma ON ma.MesaFinalId = mf.MesaFinalId
            WHERE (mf.PresidenteId = @profesorId OR mf.VocalId = @profesorId)
              AND mf.Fecha IS NOT NULL
              AND mf.FinalEstadoId IN (1, 2)
            GROUP BY mf.MesaFinalId, m.Nombre, c.Nombre, mf.Fecha, t.Descripcion, e.Descripcion, mf.FinalEstadoId
            ORDER BY mf.Fecha, m.Nombre;
            """;

        var mesas = new List<MesaFinalDocenteDto>();
        await conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                mesas.Add(new MesaFinalDocenteDto(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetInt32(6),
                    reader.GetInt32(7),
                    null));
            }
        }
        finally
        {
            conexion.Close();
        }

        return mesas;
    }

    public async Task<MesaFinalDetalleDto?> GetByIdAsync(
        int mesaId,
        int profesorId,
        CancellationToken cancellationToken = default)
    {
        const string mesaSql = """
            SELECT mf.MesaFinalId,
                   COALESCE(m.Nombre, N'Materia sin nombre'),
                   COALESCE(c.Nombre, N'Carrera sin nombre'),
                   mf.Fecha,
                   COALESCE(t.Descripcion, N'Sin turno'),
                   COALESCE(e.Descripcion, N'Sin estado'),
                   mf.FinalEstadoId
            FROM MesasFinales mf
            LEFT JOIN Materias m ON m.MateriaId = mf.MateriaId
            LEFT JOIN Carreras c ON c.CarreraId = mf.CarreraId
            LEFT JOIN Turnos t ON t.TurnoId = mf.TurnoId
            LEFT JOIN Estados e ON e.EstadoId = mf.FinalEstadoId
            WHERE mf.MesaFinalId = @mesaId
              AND (mf.PresidenteId = @profesorId OR mf.VocalId = @profesorId)
              AND mf.Fecha IS NOT NULL;
            """;

        const string estudiantesSql = """
            SELECT ma.MesaAlumnoId,
                   a.AlumnoId,
                   LTRIM(RTRIM(CONCAT(a.Apellido, N', ', a.Nombre))),
                   a.NumeroDocumento,
                   COALESCE(ma.Nota, ma.NotaDefinitiva),
                   ma.Ausente,
                   ma.Fecha
            FROM MesaAlumno ma
            INNER JOIN CursadaAlumnoCarreras cac
                ON cac.CursadaAlumnoCarreraId = ma.CursadaAlumnoCarreraId
            INNER JOIN AlumnosCarreras ac
                ON ac.AlumnoCarreraId = cac.AlumnoCarreraId
            INNER JOIN Alumnos a
                ON a.AlumnoId = ac.AlumnoId
            WHERE ma.MesaFinalId = @mesaId
            ORDER BY a.Apellido, a.Nombre;
            """;

        await conexion.OpenAsync(cancellationToken);
        try
        {
            MesaFinalDetalleDto? mesa;
            using (var command = conexion.Conector.CreateCommand())
            {
                command.CommandText = mesaSql;
                command.Parameters.Add("@mesaId", SqlDbType.Int).Value = mesaId;
                command.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
                using var reader = await command.ExecuteReaderAsync(cancellationToken);
                if (!await reader.ReadAsync(cancellationToken))
                    return null;

                mesa = new MesaFinalDetalleDto(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetInt32(6),
                    null,
                    NotaFinalRules.Minima,
                    NotaFinalRules.Maxima,
                    NotaFinalRules.Aprobacion,
                    []);
            }

            using (var command = conexion.Conector.CreateCommand())
            {
                command.CommandText = estudiantesSql;
                command.Parameters.Add("@mesaId", SqlDbType.Int).Value = mesaId;
                using var reader = await command.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    mesa.Estudiantes.Add(new EstudianteNotaFinalDto(
                        reader.GetInt32(0),
                        reader.GetInt32(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                        reader.GetBoolean(5),
                        reader.GetDateTime(6)));
                }
            }

            return mesa;
        }
        finally
        {
            conexion.Close();
        }
    }

    public async Task<bool> GuardarNotasAsync(
        int mesaId,
        int profesorId,
        IReadOnlyCollection<GuardarNotaFinalDto> notas,
        bool cerrarMesa,
        CancellationToken cancellationToken = default)
    {
        const string verificarMesaSql = """
            SELECT FinalEstadoId
            FROM MesasFinales
            WHERE MesaFinalId = @mesaId
              AND (PresidenteId = @profesorId OR VocalId = @profesorId);
            """;

        const string validarCompletitudSql = """
            SELECT COUNT(1)
            FROM MesaAlumno
            WHERE MesaFinalId = @mesaId
              AND Nota IS NULL
              AND Ausente = 0;
            """;

        const string cerrarMesaSql = """
            UPDATE MesasFinales
            SET FinalEstadoId = 2
            WHERE MesaFinalId = @mesaId
              AND (PresidenteId = @profesorId OR VocalId = @profesorId)
              AND FinalEstadoId <> 2;
            """;

        const string actualizarSql = """
            UPDATE ma
            SET ma.Nota = CASE WHEN @ausente = 1 THEN NULL ELSE @nota END,
                ma.NotaLetra = CASE WHEN @ausente = 1 THEN N'Ausente' ELSE @notaLetra END,
                ma.NotaDefinitiva = CASE WHEN @ausente = 1 THEN NULL ELSE @nota END,
                ma.Ausente = @ausente,
                ma.Estado = CASE
                    WHEN @ausente = 1 THEN @estadoAusente
                    WHEN @nota IS NULL THEN @estadoPendiente
                    WHEN @nota >= @notaAprobacion THEN @estadoAprobado
                    ELSE @estadoDesaprobado
                END,
                ma.Fecha = SYSDATETIME()
            FROM MesaAlumno ma
            WHERE ma.MesaFinalId = @mesaId
              AND ma.MesaAlumnoId = @mesaAlumnoId
              AND ma.Fecha = @ultimaModificacion;
            """;

        await conexion.OpenAsync(cancellationToken);
        await using var transaction = (SqlTransaction)await conexion.Conector.BeginTransactionAsync(cancellationToken);
        try
        {
            using (var command = conexion.Conector.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = verificarMesaSql;
                command.Parameters.Add("@mesaId", SqlDbType.Int).Value = mesaId;
                command.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
                var estadoMesa = await command.ExecuteScalarAsync(cancellationToken);
                if (estadoMesa is null || estadoMesa is DBNull)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return false;
                }

                if (Convert.ToInt32(estadoMesa) == 2)
                    throw new InvalidOperationException("La mesa final ya está cerrada y no se puede modificar.");
            }

            foreach (var nota in notas)
            {
                using var command = conexion.Conector.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = actualizarSql;
                command.Parameters.Add("@mesaId", SqlDbType.Int).Value = mesaId;
                command.Parameters.Add("@mesaAlumnoId", SqlDbType.Int).Value = nota.MesaAlumnoId;
                command.Parameters.Add("@ultimaModificacion", SqlDbType.DateTime2).Value = nota.UltimaModificacion;
                command.Parameters.Add("@ausente", SqlDbType.Bit).Value = nota.Ausente;
                command.Parameters.Add("@notaAprobacion", SqlDbType.Decimal).Value = NotaFinalRules.Aprobacion;
                command.Parameters.Add("@estadoPendiente", SqlDbType.NVarChar, 30).Value = NotaFinalRules.EstadoPendiente;
                command.Parameters.Add("@estadoAprobado", SqlDbType.NVarChar, 30).Value = NotaFinalRules.EstadoAprobado;
                command.Parameters.Add("@estadoDesaprobado", SqlDbType.NVarChar, 30).Value = NotaFinalRules.EstadoDesaprobado;
                command.Parameters.Add("@estadoAusente", SqlDbType.NVarChar, 30).Value = NotaFinalRules.EstadoAusente;
                command.Parameters.Add("@notaLetra", SqlDbType.NVarChar, 50).Value = (object?)NotaFinalRules.EnLetras(nota.Nota) ?? DBNull.Value;
                var notaParameter = command.Parameters.Add("@nota", SqlDbType.Decimal);
                notaParameter.Precision = 5;
                notaParameter.Scale = 2;
                notaParameter.Value = (object?)nota.Nota ?? DBNull.Value;

                if (await command.ExecuteNonQueryAsync(cancellationToken) != 1)
                    throw new DBConcurrencyException();
            }

            if (cerrarMesa)
            {
                using var validarCommand = conexion.Conector.CreateCommand();
                validarCommand.Transaction = transaction;
                validarCommand.CommandText = validarCompletitudSql;
                validarCommand.Parameters.Add("@mesaId", SqlDbType.Int).Value = mesaId;
                if (Convert.ToInt32(await validarCommand.ExecuteScalarAsync(cancellationToken)) > 0)
                    throw new InvalidOperationException("No se puede cerrar la mesa: hay alumnos sin nota ni ausencia.");

                using var cerrarCommand = conexion.Conector.CreateCommand();
                cerrarCommand.Transaction = transaction;
                cerrarCommand.CommandText = cerrarMesaSql;
                cerrarCommand.Parameters.Add("@mesaId", SqlDbType.Int).Value = mesaId;
                cerrarCommand.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
                if (await cerrarCommand.ExecuteNonQueryAsync(cancellationToken) != 1)
                    throw new DBConcurrencyException();
            }

            await transaction.CommitAsync(cancellationToken);
            return true;
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
        finally
        {
            conexion.Close();
        }
    }
}
