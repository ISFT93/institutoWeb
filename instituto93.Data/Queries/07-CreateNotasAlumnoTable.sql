-- Calificaciones de los alumnos para cada evaluación.
-- CursadaId forma parte de las relaciones para impedir que una nota vincule una evaluación
-- con un alumno inscripto en otra cursada.
-- Script idempotente. Requiere 06-CreateEvaluacionesTable.sql y dbo.CursadaAlumnoCarreras.

USE instituto_db;

IF OBJECT_ID(N'dbo.Evaluaciones', N'U') IS NULL
    THROW 50000, 'La tabla dbo.Evaluaciones debe existir antes de crear dbo.NotasAlumno (06-CreateEvaluacionesTable.sql).', 1;
GO

IF OBJECT_ID(N'dbo.CursadaAlumnoCarreras', N'U') IS NULL
    THROW 50000, 'La tabla dbo.CursadaAlumnoCarreras debe existir antes de crear dbo.NotasAlumno.', 1;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'UQ_CursadaAlumnoCarreras_InscripcionCursada'
      AND object_id = OBJECT_ID(N'dbo.CursadaAlumnoCarreras')
)
    CREATE UNIQUE INDEX UQ_CursadaAlumnoCarreras_InscripcionCursada
        ON dbo.CursadaAlumnoCarreras (CursadaAlumnoCarreraId, CursadaId);
GO

IF OBJECT_ID(N'dbo.NotasAlumno', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.NotasAlumno
    (
        NotaAlumnoId INT IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_NotasAlumno PRIMARY KEY,
        EvaluacionId INT NOT NULL,
        CursadaAlumnoCarreraId INT NOT NULL,
        CursadaId INT NOT NULL,
        Nota DECIMAL(4, 2) NULL,
        Ausente BIT NOT NULL
            CONSTRAINT DF_NotasAlumno_Ausente DEFAULT (0),
        Observaciones NVARCHAR(500) NULL,
        FechaCarga DATETIME2(0) NOT NULL
            CONSTRAINT DF_NotasAlumno_FechaCarga DEFAULT (SYSUTCDATETIME()),
        FechaModificacion DATETIME2(0) NULL,
        Version ROWVERSION NOT NULL,
        CONSTRAINT FK_NotasAlumno_Evaluaciones
            FOREIGN KEY (EvaluacionId, CursadaId)
            REFERENCES dbo.Evaluaciones (EvaluacionId, CursadaId),
        CONSTRAINT FK_NotasAlumno_CursadaAlumnoCarreras
            FOREIGN KEY (CursadaAlumnoCarreraId, CursadaId)
            REFERENCES dbo.CursadaAlumnoCarreras (CursadaAlumnoCarreraId, CursadaId),
        CONSTRAINT UQ_NotasAlumno_EvaluacionAlumno
            UNIQUE (EvaluacionId, CursadaAlumnoCarreraId),
        CONSTRAINT CK_NotasAlumno_Nota
            CHECK (Nota IS NULL OR Nota BETWEEN 0 AND 10),
        CONSTRAINT CK_NotasAlumno_Ausencia
            CHECK (Ausente = 0 OR Nota IS NULL)
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_NotasAlumno_CursadaAlumnoCarreraId'
      AND object_id = OBJECT_ID(N'dbo.NotasAlumno')
)
    CREATE INDEX IX_NotasAlumno_CursadaAlumnoCarreraId
        ON dbo.NotasAlumno (CursadaAlumnoCarreraId);
GO
