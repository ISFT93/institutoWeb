-- Evaluaciones parciales pertenecientes a una cursada concreta.
-- Script idempotente. Requiere dbo.Cursadas y 05-CreateTiposEvaluacionTable.sql.

IF OBJECT_ID(N'dbo.Cursadas', N'U') IS NULL
    THROW 50000, 'La tabla dbo.Cursadas debe existir antes de crear dbo.Evaluaciones.', 1;
GO

IF OBJECT_ID(N'dbo.TiposEvaluacion', N'U') IS NULL
    THROW 50000, 'La tabla dbo.TiposEvaluacion debe existir antes de crear dbo.Evaluaciones (05-CreateTiposEvaluacionTable.sql).', 1;
GO

IF OBJECT_ID(N'dbo.Evaluaciones', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Evaluaciones
    (
        EvaluacionId INT IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_Evaluaciones PRIMARY KEY,
        CursadaId INT NOT NULL,
        TipoEvaluacionId INT NOT NULL,
        Nombre NVARCHAR(100) NOT NULL,
        Fecha DATE NOT NULL,
        Cuatrimestre TINYINT NOT NULL,
        Estado NVARCHAR(15) NOT NULL
            CONSTRAINT DF_Evaluaciones_Estado DEFAULT (N'Borrador'),
        Ponderacion DECIMAL(5, 2) NULL,
        FechaCreacion DATETIME2(0) NOT NULL
            CONSTRAINT DF_Evaluaciones_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        FechaModificacion DATETIME2(0) NULL,
        Version ROWVERSION NOT NULL,
        CONSTRAINT FK_Evaluaciones_Cursadas
            FOREIGN KEY (CursadaId) REFERENCES dbo.Cursadas (CursadaId),
        CONSTRAINT FK_Evaluaciones_TiposEvaluacion
            FOREIGN KEY (TipoEvaluacionId) REFERENCES dbo.TiposEvaluacion (TipoEvaluacionId),
        CONSTRAINT CK_Evaluaciones_Nombre
            CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
        CONSTRAINT CK_Evaluaciones_Cuatrimestre
            CHECK (Cuatrimestre IN (1, 2)),
        CONSTRAINT CK_Evaluaciones_Estado
            CHECK (Estado IN (N'Borrador', N'Activa', N'Cerrada')),
        CONSTRAINT CK_Evaluaciones_Ponderacion
            CHECK (Ponderacion IS NULL OR Ponderacion BETWEEN 0 AND 100),
        CONSTRAINT UQ_Evaluaciones_EvaluacionCursada
            UNIQUE (EvaluacionId, CursadaId)
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Evaluaciones_CursadaId_Estado_Fecha'
      AND object_id = OBJECT_ID(N'dbo.Evaluaciones')
)
    CREATE INDEX IX_Evaluaciones_CursadaId_Estado_Fecha
        ON dbo.Evaluaciones (CursadaId, Estado, Fecha);
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Evaluaciones_TipoEvaluacionId'
      AND object_id = OBJECT_ID(N'dbo.Evaluaciones')
)
    CREATE INDEX IX_Evaluaciones_TipoEvaluacionId
        ON dbo.Evaluaciones (TipoEvaluacionId);
GO
