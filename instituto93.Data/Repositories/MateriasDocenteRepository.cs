using System.Data;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.DTOs;
using instituto93.Domain.Rules;

namespace instituto93.Data.Repositories;

public sealed class MateriasDocenteRepository(Conexion conexion) : IMateriasDocenteRepository
{
    private const string PermisoSql = """EXISTS (SELECT 1 FROM Servicios s INNER JOIN Cursadas cp ON cp.CursoMateriaId=s.CursoMateriaId WHERE cp.CursadaId=e.CursadaId AND s.PersonalId=@profesorId AND s.Activo=1 AND s.FechaBaja IS NULL)""";

    public async Task<List<MateriaDocenteDto>> GetByProfesorAsync(int profesorId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT c.CursadaId, m.Nombre, COALESCE(ca.Nombre, N'Carrera sin nombre'),
                   COALESCE(cu.NombreCurso, N'Curso sin nombre'), COALESCE(c.AnioLectivo, YEAR(GETDATE())),
                   COUNT(DISTINCT CASE WHEN ISNULL(cac.Activo, 1) = 1 THEN cac.CursadaAlumnoCarreraId END),
                   COUNT(DISTINCT e.EvaluacionId)
            FROM Cursadas c
            INNER JOIN CursoMaterias cm ON cm.CursoMateriaId = c.CursoMateriaId
            INNER JOIN Materias m ON m.MateriaId = cm.MateriaId
            INNER JOIN Cursos cu ON cu.CursoId = cm.CursoId
            LEFT JOIN Carreras ca ON ca.CarreraId = m.CarreraId
            LEFT JOIN CursadaAlumnoCarreras cac ON cac.CursadaId = c.CursadaId
            LEFT JOIN Evaluaciones e ON e.CursadaId = c.CursadaId
            WHERE EXISTS (SELECT 1 FROM Servicios s
                          WHERE s.CursoMateriaId = c.CursoMateriaId AND s.PersonalId = @profesorId
                            AND s.Activo = 1 AND s.FechaBaja IS NULL)
              AND ISNULL(cm.Activo, 1) = 1 AND m.Activo = 1
            GROUP BY c.CursadaId, m.Nombre, ca.Nombre, cu.NombreCurso, c.AnioLectivo
            ORDER BY c.AnioLectivo DESC, m.Nombre;
            """;

        var result = new List<MateriaDocenteDto>();
        await conexion.OpenAsync(cancellationToken);
        try
        {
            using var command = conexion.Conector.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
                result.Add(new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                    reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6)));
        }
        finally { conexion.Close(); }
        return result;
    }

    public async Task<MateriaDocenteDetalleDto?> GetByIdAsync(int cursadaId, int profesorId, CancellationToken cancellationToken = default)
    {
        const string cabeceraSql = """
            SELECT c.CursadaId, m.Nombre, COALESCE(ca.Nombre, N'Carrera sin nombre'),
                   COALESCE(cu.NombreCurso, N'Curso sin nombre'), COALESCE(c.AnioLectivo, YEAR(GETDATE())), m.CargaHoraria
            FROM Cursadas c INNER JOIN CursoMaterias cm ON cm.CursoMateriaId=c.CursoMateriaId
            INNER JOIN Materias m ON m.MateriaId=cm.MateriaId INNER JOIN Cursos cu ON cu.CursoId=cm.CursoId
            LEFT JOIN Carreras ca ON ca.CarreraId=m.CarreraId
            WHERE c.CursadaId=@cursadaId AND EXISTS
                (SELECT 1 FROM Servicios s WHERE s.CursoMateriaId=c.CursoMateriaId AND s.PersonalId=@profesorId
                 AND s.Activo=1 AND s.FechaBaja IS NULL);
            """;
        const string alumnosSql = """
            SELECT cac.CursadaAlumnoCarreraId, a.AlumnoId,
                   LTRIM(RTRIM(CONCAT(a.Apellido, N', ', a.Nombre))), a.NumeroDocumento,
                   COALESCE(NULLIF(LTRIM(RTRIM(cac.Estado)), N''), N'Activo'), cac.PorcentajeAsistencia
            FROM CursadaAlumnoCarreras cac INNER JOIN AlumnosCarreras ac ON ac.AlumnoCarreraId=cac.AlumnoCarreraId
            INNER JOIN Alumnos a ON a.AlumnoId=ac.AlumnoId
            WHERE cac.CursadaId=@cursadaId AND ISNULL(cac.Activo,1)=1
            ORDER BY a.Apellido,a.Nombre;
            """;
        await conexion.OpenAsync(cancellationToken);
        try
        {
            MateriaDocenteDetalleDto detalle;
            using (var command = conexion.Conector.CreateCommand())
            {
                command.CommandText = cabeceraSql;
                command.Parameters.Add("@cursadaId", SqlDbType.Int).Value = cursadaId;
                command.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
                using var reader = await command.ExecuteReaderAsync(cancellationToken);
                if (!await reader.ReadAsync(cancellationToken)) return null;
                detalle = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4),
                    reader.IsDBNull(5) ? null : reader.GetInt32(5), []);
            }
            using (var command = conexion.Conector.CreateCommand())
            {
                command.CommandText = alumnosSql;
                command.Parameters.Add("@cursadaId", SqlDbType.Int).Value = cursadaId;
                using var reader = await command.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                    detalle.Estudiantes.Add(new(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                        reader.IsDBNull(5) ? null : reader.GetDecimal(5)));
            }
            return detalle;
        }
        finally { conexion.Close(); }
    }

    public async Task<List<TipoEvaluacionDto>> GetTiposAsync(CancellationToken ct = default)
    {
        const string sql = "SELECT TipoEvaluacionId,Codigo,Descripcion FROM TiposEvaluacion WHERE Activo=1 ORDER BY Descripcion";

        var r = new List<TipoEvaluacionDto>();
        await conexion.OpenAsync(ct);
        try
        {
            using var c = conexion.Conector.CreateCommand();
            c.CommandText = sql;
            using var d = await c.ExecuteReaderAsync(ct);
            while (await d.ReadAsync(ct))
                r.Add(new(d.GetInt32(0), d.GetString(1), d.GetString(2)));
        }
        finally { conexion.Close(); }
        return r;
    }

    public async Task<List<EvaluacionDocenteDto>?> GetEvaluacionesAsync(int cursadaId, int profesorId, CancellationToken ct = default)
    {
        const string sql = """
            SELECT CASE WHEN EXISTS(SELECT 1 FROM Servicios s INNER JOIN Cursadas c ON c.CursoMateriaId=s.CursoMateriaId WHERE c.CursadaId=@cursadaId AND s.PersonalId=@profesorId AND s.Activo=1 AND s.FechaBaja IS NULL) THEN 1 ELSE 0 END;
            SELECT e.EvaluacionId,e.TipoEvaluacionId,e.Nombre,t.Descripcion,e.Fecha,e.Cuatrimestre,e.Estado,e.Ponderacion,e.Version,COUNT(CASE WHEN n.Nota IS NOT NULL OR n.Ausente=1 THEN 1 END),(SELECT COUNT(*) FROM CursadaAlumnoCarreras x WHERE x.CursadaId=e.CursadaId AND ISNULL(x.Activo,1)=1)
            FROM Evaluaciones e INNER JOIN TiposEvaluacion t ON t.TipoEvaluacionId=e.TipoEvaluacionId LEFT JOIN NotasAlumno n ON n.EvaluacionId=e.EvaluacionId
            WHERE e.CursadaId=@cursadaId GROUP BY e.EvaluacionId,e.TipoEvaluacionId,e.Nombre,t.Descripcion,e.Fecha,e.Cuatrimestre,e.Estado,e.Ponderacion,e.Version,e.CursadaId ORDER BY e.Fecha;
            """;
        var result = new List<EvaluacionDocenteDto>();
        await conexion.OpenAsync(ct);
        try
        {
            using var c = conexion.Conector.CreateCommand();
            c.CommandText = sql;
            c.Parameters.Add("@cursadaId", SqlDbType.Int).Value = cursadaId;
            c.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
            using var d = await c.ExecuteReaderAsync(ct);
            if (!await d.ReadAsync(ct) || d.GetInt32(0) == 0)
                return null;

            await d.NextResultAsync(ct);
            while (await d.ReadAsync(ct))
                result.Add(new(
                    d.GetInt32(0), d.GetInt32(1), d.GetString(2), d.GetString(3), d.GetDateTime(4), d.GetByte(5), d.GetString(6),
                    d.IsDBNull(7) ? null : d.GetDecimal(7),
                    Convert.ToBase64String((byte[])d[8]),
                    d.GetInt32(9), d.GetInt32(10)));
        }
        finally { conexion.Close(); }
        return result;
    }

    public async Task<int?> CrearEvaluacionAsync(int cursadaId, int profesorId, CrearEvaluacionDto r, CancellationToken ct = default)
    {
        const string sql = """IF NOT EXISTS(SELECT 1 FROM Servicios s INNER JOIN Cursadas c ON c.CursoMateriaId=s.CursoMateriaId WHERE c.CursadaId=@cursadaId AND s.PersonalId=@profesorId AND s.Activo=1 AND s.FechaBaja IS NULL) RETURN; INSERT INTO Evaluaciones(CursadaId,TipoEvaluacionId,Nombre,Fecha,Cuatrimestre) OUTPUT INSERTED.EvaluacionId VALUES(@cursadaId,@tipo,@nombre,@fecha,@cuatri);""";

        await conexion.OpenAsync(ct);
        try
        {
            using var c = conexion.Conector.CreateCommand();
            c.CommandText = sql;
            c.Parameters.Add("@cursadaId", SqlDbType.Int).Value = cursadaId;
            c.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
            c.Parameters.Add("@tipo", SqlDbType.Int).Value = r.TipoEvaluacionId;
            c.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = r.Nombre!.Trim();
            c.Parameters.Add("@fecha", SqlDbType.Date).Value = r.Fecha.Date;
            c.Parameters.Add("@cuatri", SqlDbType.TinyInt).Value = r.Cuatrimestre;
            var x = await c.ExecuteScalarAsync(ct);
            return x is null ? null : Convert.ToInt32(x);
        }
        finally { conexion.Close(); }
    }

    public async Task<bool> ActualizarEvaluacionAsync(int id,int profesorId,ActualizarEvaluacionDto r,CancellationToken ct=default)
    {
        const string actualizarSql =
            """UPDATE e SET TipoEvaluacionId=@tipo,Nombre=@nombre,Fecha=@fecha,Cuatrimestre=@cuatri,Estado=@estado,Ponderacion=@ponderacion,FechaModificacion=SYSUTCDATETIME() FROM Evaluaciones e WHERE e.EvaluacionId=@id AND e.Version=@version AND """
            + PermisoSql;
        const string cerrarSql = """
            UPDATE n
               SET n.Nota = NULL,
                   n.Ausente = 1,
                   n.FechaModificacion = SYSUTCDATETIME()
            FROM NotasAlumno n
            INNER JOIN Evaluaciones e ON e.EvaluacionId = n.EvaluacionId
            INNER JOIN CursadaAlumnoCarreras cac
                    ON cac.CursadaAlumnoCarreraId = n.CursadaAlumnoCarreraId
                   AND cac.CursadaId = e.CursadaId
            WHERE n.EvaluacionId = @id
              AND n.Nota IS NULL
              AND n.Ausente = 0
              AND ISNULL(cac.Activo, 1) = 1;

            INSERT INTO NotasAlumno
                (EvaluacionId, CursadaAlumnoCarreraId, CursadaId, Nota, Ausente, Observaciones)
            SELECT e.EvaluacionId,
                   cac.CursadaAlumnoCarreraId,
                   e.CursadaId,
                   NULL,
                   1,
                   NULL
            FROM Evaluaciones e
            INNER JOIN CursadaAlumnoCarreras cac ON cac.CursadaId = e.CursadaId
            WHERE e.EvaluacionId = @id
              AND ISNULL(cac.Activo, 1) = 1
              AND NOT EXISTS
                  (SELECT 1
                   FROM NotasAlumno n
                   WHERE n.EvaluacionId = e.EvaluacionId
                     AND n.CursadaAlumnoCarreraId = cac.CursadaAlumnoCarreraId);
            """;

        await conexion.OpenAsync(ct);
        await using var tx = await conexion.Conector.BeginTransactionAsync(ct);
        try
        {
            using (var c = conexion.Conector.CreateCommand())
            {
                c.Transaction = (Microsoft.Data.SqlClient.SqlTransaction)tx;
                c.CommandText = actualizarSql;
                c.Parameters.Add("@id", SqlDbType.Int).Value = id;
                c.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
                c.Parameters.Add("@tipo", SqlDbType.Int).Value = r.TipoEvaluacionId;
                c.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = r.Nombre!.Trim();
                c.Parameters.Add("@fecha", SqlDbType.Date).Value = r.Fecha.Date;
                c.Parameters.Add("@cuatri", SqlDbType.TinyInt).Value = r.Cuatrimestre;
                c.Parameters.Add("@estado", SqlDbType.NVarChar, 15).Value = r.Estado!;
                var ponderacion = c.Parameters.Add("@ponderacion", SqlDbType.Decimal);
                ponderacion.Precision = 5;
                ponderacion.Scale = 2;
                ponderacion.Value = (object?)r.Ponderacion ?? DBNull.Value;
                c.Parameters.Add("@version", SqlDbType.Timestamp).Value = DecodificarVersion(r.Version!);

                if (await c.ExecuteNonQueryAsync(ct) != 1)
                {
                    await tx.RollbackAsync(ct);
                    return false;
                }
            }

            if (string.Equals(r.Estado, "Cerrada", StringComparison.Ordinal))
            {
                using var c = conexion.Conector.CreateCommand();
                c.Transaction = (Microsoft.Data.SqlClient.SqlTransaction)tx;
                c.CommandText = cerrarSql;
                c.Parameters.Add("@id", SqlDbType.Int).Value = id;
                await c.ExecuteNonQueryAsync(ct);
            }

            await tx.CommitAsync(ct);
            return true;
        }
        catch
        {
            await tx.RollbackAsync(CancellationToken.None);
            throw;
        }
        finally { conexion.Close(); }
    }

    public async Task<EvaluacionNotasDto?> GetNotasAsync(int evaluacionId, int profesorId, CancellationToken ct = default)
    {
        const string sql =
            """SELECT e.EvaluacionId,e.CursadaId,e.Nombre,t.Descripcion,e.Fecha,e.Cuatrimestre,e.Estado FROM Evaluaciones e INNER JOIN TiposEvaluacion t ON t.TipoEvaluacionId=e.TipoEvaluacionId WHERE e.EvaluacionId=@id AND """
            + PermisoSql +
            """; SELECT cac.CursadaAlumnoCarreraId,a.AlumnoId,LTRIM(RTRIM(CONCAT(a.Apellido,N', ',a.Nombre))),a.NumeroDocumento,n.Nota,ISNULL(n.Ausente,0),n.Observaciones,n.Version FROM Evaluaciones e INNER JOIN CursadaAlumnoCarreras cac ON cac.CursadaId=e.CursadaId INNER JOIN AlumnosCarreras ac ON ac.AlumnoCarreraId=cac.AlumnoCarreraId INNER JOIN Alumnos a ON a.AlumnoId=ac.AlumnoId LEFT JOIN NotasAlumno n ON n.EvaluacionId=e.EvaluacionId AND n.CursadaAlumnoCarreraId=cac.CursadaAlumnoCarreraId WHERE e.EvaluacionId=@id AND ISNULL(cac.Activo,1)=1 ORDER BY a.Apellido,a.Nombre;""";

        await conexion.OpenAsync(ct);
        try
        {
            using var c = conexion.Conector.CreateCommand();
            c.CommandText = sql;
            c.Parameters.Add("@id", SqlDbType.Int).Value = evaluacionId;
            c.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
            using var d = await c.ExecuteReaderAsync(ct);
            if (!await d.ReadAsync(ct))
                return null;

            var r = new EvaluacionNotasDto(
                d.GetInt32(0), d.GetInt32(1), d.GetString(2), d.GetString(3), d.GetDateTime(4), d.GetByte(5), d.GetString(6),
                NotaEvaluacionRules.Minima, NotaEvaluacionRules.Maxima, NotaEvaluacionRules.Aprobacion, []);
            await d.NextResultAsync(ct);
            while (await d.ReadAsync(ct))
                r.Estudiantes.Add(new(
                    d.GetInt32(0), d.GetInt32(1), d.GetString(2), d.GetString(3),
                    d.IsDBNull(4) ? null : d.GetDecimal(4),
                    d.GetBoolean(5),
                    d.IsDBNull(6) ? null : d.GetString(6),
                    d.IsDBNull(7) ? null : Convert.ToBase64String((byte[])d[7])));
            return r;
        }
        finally { conexion.Close(); }
    }

    public async Task<bool> GuardarNotasAsync(int evaluacionId, int profesorId, IReadOnlyCollection<GuardarNotaAlumnoDto> notas, CancellationToken ct = default)
    {
        const string verify = "SELECT CursadaId FROM Evaluaciones e WITH (UPDLOCK, HOLDLOCK) WHERE EvaluacionId=@id AND Estado=N'Activa' AND " + PermisoSql;
        const string sql = """UPDATE NotasAlumno SET Nota=CASE WHEN @ausente=1 THEN NULL ELSE @nota END,Ausente=@ausente,Observaciones=@obs,FechaModificacion=SYSUTCDATETIME() WHERE EvaluacionId=@id AND CursadaAlumnoCarreraId=@inscripcion AND (@version IS NULL OR Version=@version); IF @@ROWCOUNT=0 AND @version IS NULL INSERT INTO NotasAlumno(EvaluacionId,CursadaAlumnoCarreraId,CursadaId,Nota,Ausente,Observaciones) SELECT @id,@inscripcion,@cursada,CASE WHEN @ausente=1 THEN NULL ELSE @nota END,@ausente,@obs WHERE EXISTS(SELECT 1 FROM CursadaAlumnoCarreras WHERE CursadaAlumnoCarreraId=@inscripcion AND CursadaId=@cursada);""";

        await conexion.OpenAsync(ct);
        await using var tx = await conexion.Conector.BeginTransactionAsync(ct);
        try
        {
            int cursada;
            using (var c = conexion.Conector.CreateCommand())
            {
                c.Transaction = (Microsoft.Data.SqlClient.SqlTransaction)tx;
                c.CommandText = verify;
                c.Parameters.Add("@id", SqlDbType.Int).Value = evaluacionId;
                c.Parameters.Add("@profesorId", SqlDbType.Int).Value = profesorId;
                var x = await c.ExecuteScalarAsync(ct);
                if (x is null)
                {
                    await tx.RollbackAsync(ct);
                    return false;
                }
                cursada = Convert.ToInt32(x);
            }

            foreach (var n in notas)
            {
                using var c = conexion.Conector.CreateCommand();
                c.Transaction = (Microsoft.Data.SqlClient.SqlTransaction)tx;
                c.CommandText = sql;
                c.Parameters.Add("@id", SqlDbType.Int).Value = evaluacionId;
                c.Parameters.Add("@inscripcion", SqlDbType.Int).Value = n.CursadaAlumnoCarreraId;
                c.Parameters.Add("@cursada", SqlDbType.Int).Value = cursada;
                var nota = c.Parameters.Add("@nota", SqlDbType.Decimal);
                nota.Precision = 4;
                nota.Scale = 2;
                nota.Value = (object?)n.Nota ?? DBNull.Value;
                c.Parameters.Add("@ausente", SqlDbType.Bit).Value = n.Ausente;
                c.Parameters.Add("@obs", SqlDbType.NVarChar, 500).Value = (object?)n.Observaciones ?? DBNull.Value;
                c.Parameters.Add("@version", SqlDbType.Timestamp).Value = n.Version is null ? DBNull.Value : DecodificarVersion(n.Version);
                if (await c.ExecuteNonQueryAsync(ct) == 0)
                    throw new DBConcurrencyException();
            }

            await tx.CommitAsync(ct);
            return true;
        }
        catch
        {
            await tx.RollbackAsync(CancellationToken.None);
            throw;
        }
        finally { conexion.Close(); }
    }

    private static byte[] DecodificarVersion(string version) =>
        version.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
            ? Convert.FromHexString(version[2..])
            : Convert.FromBase64String(version);

}

