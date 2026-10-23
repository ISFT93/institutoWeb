-- Inscripciones y calificaciones de los alumnos en mesas finales.
-- Cada inscripción de cursada puede aparecer una sola vez dentro de una mesa.
-- Script idempotente. Requiere dbo.MesasFinales y dbo.CursadaAlumnoCarreras.

IF OBJECT_ID(N'dbo.MesasFinales', N'U') IS NULL
    THROW 50000, 'La tabla dbo.MesasFinales debe existir antes de crear dbo.MesaAlumno.', 1;
GO

IF OBJECT_ID(N'dbo.CursadaAlumnoCarreras', N'U') IS NULL
    THROW 50000, 'La tabla dbo.CursadaAlumnoCarreras debe existir antes de crear dbo.MesaAlumno.', 1;
GO

IF OBJECT_ID(N'dbo.MesaAlumno', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.MesaAlumno
    (
        MesaAlumnoId INT IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_MesaAlumno PRIMARY KEY,
        MesaFinalId INT NOT NULL,
        CursadaAlumnoCarreraId INT NOT NULL,
        Nota DECIMAL(4, 2) NULL,
        NotaLetra NVARCHAR(50) NULL,
        NotaDefinitiva DECIMAL(4, 2) NULL,
        Estado NVARCHAR(30) NOT NULL
            CONSTRAINT DF_MesaAlumno_Estado DEFAULT (N'Pendiente'),
        Fecha DATETIME2(7) NOT NULL
            CONSTRAINT DF_MesaAlumno_Fecha DEFAULT (SYSDATETIME()),
        CONSTRAINT FK_MesaAlumno_MesasFinales
            FOREIGN KEY (MesaFinalId)
            REFERENCES dbo.MesasFinales (MesaFinalId),
        CONSTRAINT FK_MesaAlumno_CursadaAlumnoCarreras
            FOREIGN KEY (CursadaAlumnoCarreraId)
            REFERENCES dbo.CursadaAlumnoCarreras (CursadaAlumnoCarreraId),
        CONSTRAINT UQ_MesaAlumno_Mesa_CursadaAlumnoCarrera
            UNIQUE (MesaFinalId, CursadaAlumnoCarreraId),
        CONSTRAINT CK_MesaAlumno_Nota
            CHECK (Nota IS NULL OR Nota BETWEEN 0 AND 10),
        CONSTRAINT CK_MesaAlumno_NotaDefinitiva
            CHECK (NotaDefinitiva IS NULL OR NotaDefinitiva BETWEEN 0 AND 10),
        CONSTRAINT CK_MesaAlumno_Estado
            CHECK (LEN(LTRIM(RTRIM(Estado))) > 0)
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_MesaAlumno_CursadaAlumnoCarreraId'
      AND object_id = OBJECT_ID(N'dbo.MesaAlumno')
)
    CREATE INDEX IX_MesaAlumno_CursadaAlumnoCarreraId
        ON dbo.MesaAlumno (CursadaAlumnoCarreraId);
GO
