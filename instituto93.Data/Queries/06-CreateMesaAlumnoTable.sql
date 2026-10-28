-- Vinculación entre alumnos y mesas finales (inscripciones).
-- Script idempotente. Requiere dbo.Alumnos y dbo.MesasFinales.

IF OBJECT_ID(N'dbo.Alumnos', N'U') IS NULL
    THROW 50000, 'La tabla dbo.Alumnos debe existir antes de ejecutar 06-CreateMesaAlumnoTable.sql.', 1;
GO

IF OBJECT_ID(N'dbo.MesasFinales', N'U') IS NULL
    THROW 50000, 'La tabla dbo.MesasFinales debe existir antes de ejecutar 06-CreateMesaAlumnoTable.sql.', 1;
GO

IF OBJECT_ID(N'dbo.MesaAlumno', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.MesaAlumno
    (
        MesaAlumnoId INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_MesaAlumno PRIMARY KEY,
        MesaFinalId INT NOT NULL,
        AlumnoId INT NOT NULL,
        FechaInscripcion DATETIME2(0) NOT NULL CONSTRAINT DF_MesaAlumno_FechaInscripcion DEFAULT (GETDATE()),
        CONSTRAINT FK_MesaAlumno_MesasFinales FOREIGN KEY (MesaFinalId) REFERENCES dbo.MesasFinales (MesaFinalId),
        CONSTRAINT FK_MesaAlumno_Alumnos FOREIGN KEY (AlumnoId) REFERENCES dbo.Alumnos (AlumnoId)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_MesaAlumno_MesaFinalId_AlumnoId' AND object_id = OBJECT_ID(N'dbo.MesaAlumno'))
    EXEC (N'CREATE UNIQUE INDEX UX_MesaAlumno_MesaFinalId_AlumnoId ON dbo.MesaAlumno (MesaFinalId, AlumnoId);');
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MesaAlumno_AlumnoId' AND object_id = OBJECT_ID(N'dbo.MesaAlumno'))
    EXEC (N'CREATE INDEX IX_MesaAlumno_AlumnoId ON dbo.MesaAlumno (AlumnoId);');
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MesaAlumno_MesaFinalId' AND object_id = OBJECT_ID(N'dbo.MesaAlumno'))
    EXEC (N'CREATE INDEX IX_MesaAlumno_MesaFinalId ON dbo.MesaAlumno (MesaFinalId);');
GO
