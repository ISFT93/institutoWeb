-- Sedes físicas del instituto y FK desde dbo.MesasFinales.SedeId.
-- También normaliza dbo.MesasFinales.Fecha a DATETIME2(0) (era DATE).
-- Script idempotente. Requiere dbo.MesasFinales.


-- Verifica que la tabla principal exista antes de continuar.
-- Si no existe, se detiene la ejecución del script con un error.
IF OBJECT_ID(N'dbo.MesasFinales', N'U') IS NULL
    THROW 50000, 'La tabla dbo.MesasFinales debe existir antes de ejecutar 05-CreateSedesTable.sql.', 1;
GO

-- Crea la tabla de sedes únicamente si todavía no existe.
IF OBJECT_ID(N'dbo.Sedes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Sedes
    (
        SedeId INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_Sedes PRIMARY KEY,
        Descripcion NVARCHAR(100) NOT NULL
    );
END;
GO

-- Inserta las sedes iniciales del sistema.
-- El WHERE NOT EXISTS evita duplicados si el script se ejecuta
-- más de una vez.
INSERT INTO dbo.Sedes (Descripcion)
SELECT v.Descripcion
FROM (VALUES
    (N'Sede San Vicente'),
    (N'Sede Enfermería – E.E.S. Nº 5 (Alejandro Korn)'),
    (N'UNSaV – Universidad Nacional de San Vicente')
) AS v (Descripcion)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Sedes s WHERE s.Descripcion = v.Descripcion);
GO

-- Agrega la columna SedeId a MesasFinales únicamente
-- si todavía no existe.
IF COL_LENGTH(N'dbo.MesasFinales', N'SedeId') IS NULL
BEGIN
    EXEC (N'ALTER TABLE dbo.MesasFinales ADD SedeId INT NULL;');
    EXEC (N'ALTER TABLE dbo.MesasFinales ADD CONSTRAINT FK_MesasFinales_SedeId FOREIGN KEY (SedeId) REFERENCES dbo.Sedes (SedeId);');
END
ELSE
BEGIN
    IF OBJECT_ID(N'dbo.FK_MesasFinales_SedeId', N'C') IS NULL
        EXEC (N'ALTER TABLE dbo.MesasFinales ADD CONSTRAINT FK_MesasFinales_SedeId FOREIGN KEY (SedeId) REFERENCES dbo.Sedes (SedeId);');
END
GO

-- Si la columna Fecha todavía es de tipo DATE,
-- la convierte a DATETIME2(0).
-- De esta forma permite ingresar fecha y hora.
IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.MesasFinales')
      AND name = N'Fecha'
      AND TYPE_NAME(system_type_id) = N'date')
    EXEC (N'ALTER TABLE dbo.MesasFinales ALTER COLUMN Fecha DATETIME2(0);');
GO
