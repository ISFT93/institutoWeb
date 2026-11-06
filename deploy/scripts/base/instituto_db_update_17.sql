
USE instituto_db;
GO

/* ============================================================================
   Creación de la tabla de catálogo Regimenes (Modalidades) e Insert Inicial
   ============================================================================ */
IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Regimenes'
)
BEGIN
    CREATE TABLE dbo.Regimenes (
        RegimenId INT IDENTITY(1,1) PRIMARY KEY,
        Nombre VARCHAR(50) NOT NULL UNIQUE,       -- 'Anual', 'Cuatrimestral', 'Otro'
        MultiplicadorHoras INT NULL,               -- 32 para Anual, 16 para Cuatrimestral
        Activo BIT NOT NULL CONSTRAINT DF_Regimenes_Activo DEFAULT 1
    );

    PRINT 'Tabla dbo.Regimenes creada correctamente.';
END
GO

-- Insert idempotente de los datos de catálogo iniciales
IF NOT EXISTS (SELECT 1 FROM dbo.Regimenes WHERE Nombre = 'Anual')
BEGIN
    INSERT INTO dbo.Regimenes (Nombre, MultiplicadorHoras, Activo) VALUES ('Anual', 32, 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Regimenes WHERE Nombre = 'Cuatrimestral')
BEGIN
    INSERT INTO dbo.Regimenes (Nombre, MultiplicadorHoras, Activo) VALUES ('Cuatrimestral', 16, 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Regimenes WHERE Nombre = 'Otro')
BEGIN
    INSERT INTO dbo.Regimenes (Nombre, MultiplicadorHoras, Activo) VALUES ('Otro', NULL, 1);
END
GO


/* ============================================================================
   Agregar RegimenId (FK) a la tabla Carreras
   ============================================================================ */
IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Carreras' AND COLUMN_NAME = 'RegimenId'
)
BEGIN
    ALTER TABLE dbo.Carreras 
    ADD RegimenId INT NOT NULL CONSTRAINT DF_Carreras_RegimenId DEFAULT 1;

    ALTER TABLE dbo.Carreras 
    ADD CONSTRAINT FK_Carreras_Regimenes 
    FOREIGN KEY (RegimenId) REFERENCES dbo.Regimenes(RegimenId);

    PRINT 'Columna RegimenId y FK agregadas a dbo.Carreras.';
END
GO


/* ============================================================================
   Agregar RegimenId (FK) y Modulo a la tabla Materias
   ============================================================================ */

-- Agregamos la clave foránea a Regimenes
IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Materias' AND COLUMN_NAME = 'RegimenId'
)
BEGIN
    ALTER TABLE dbo.Materias 
    ADD RegimenId INT NULL;

    ALTER TABLE dbo.Materias 
    ADD CONSTRAINT FK_Materias_Regimenes 
    FOREIGN KEY (RegimenId) REFERENCES dbo.Regimenes(RegimenId);

    PRINT 'Columna RegimenId y FK agregadas a dbo.Materias.';
END
GO

-- Agregamos el campo Modulo
IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Materias' AND COLUMN_NAME = 'Modulo'
)
BEGIN
    ALTER TABLE dbo.Materias 
    ADD Modulo INT NULL;

    PRINT 'Columna Modulo agregada a dbo.Materias.';
END
GO


/* ============================================================================
   Consulta de verificación de la estructura unificada
   ============================================================================ */
SELECT 
    M.MateriaId,
    M.Nombre AS Materia,
    M.CargaHoraria,
    M.Modulo,
    R.Nombre AS Modalidad,
    E.Descripcion AS Espacio
FROM dbo.Materias M
LEFT JOIN dbo.Regimenes R ON M.RegimenId = R.RegimenId
LEFT JOIN dbo.Espacios E ON M.EspacioId = E.EspacioId;
GO