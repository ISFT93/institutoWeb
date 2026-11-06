USE instituto_db;
GO

/* ============================================================================
      CREAR LA TABLA (SI NO EXISTE)
   Se crea una estructura base que se irá perfeccionando en los siguientes pasos
   ============================================================================ */
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Espacios')
BEGIN
    CREATE TABLE dbo.Espacios (
        EspacioId INT IDENTITY(1,1) PRIMARY KEY,
        Descripcion VARCHAR(100) NOT NULL
    );
END
GO

/* ============================================================================
      ELIMINAR COLUMNAS U OBSOLETOS (SI EXISTEN)
   ============================================================================ */
-- Eliminar la columna 'Acumulador' si se encuentra en la tabla
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Espacios' AND COLUMN_NAME = 'Acumulador')
BEGIN
    ALTER TABLE dbo.Espacios DROP COLUMN Acumulador;
END
GO

-- Eliminar restricciones (DEFAULT) viejas de SumaHoras y CalculaPorcentaje para evitar errores
DECLARE @Command NVARCHAR(MAX) = '';
SELECT @Command = @Command + 'ALTER TABLE dbo.Espacios DROP CONSTRAINT [' + name + '];' 
FROM sys.default_constraints 
WHERE parent_object_id = OBJECT_ID('dbo.Espacios') 
  AND parent_column_id IN (
      SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Espacios') AND name IN ('SumaHoras', 'CalculaPorcentaje')
  );
IF @Command <> '' EXEC(@Command);
GO

/* ============================================================================
      ASEGURAR QUE LAS COLUMNAS EXISTAN CON EL TIPO DE DATO CORRECTO
   ============================================================================ */
-- Asegurar que Descripción soporte textos largos
ALTER TABLE dbo.Espacios ALTER COLUMN Descripcion VARCHAR(100) NOT NULL;
GO

-- Crear o modificar SumaHoras (Debe ser INT numérico)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Espacios' AND COLUMN_NAME = 'SumaHoras')
BEGIN
    ALTER TABLE dbo.Espacios ADD SumaHoras INT NOT NULL CONSTRAINT DF_Espacios_SumaHoras_Num DEFAULT 0;
END
ELSE
BEGIN
    ALTER TABLE dbo.Espacios ALTER COLUMN SumaHoras INT NOT NULL;
    IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('dbo.Espacios') AND name = 'DF_Espacios_SumaHoras_Num')
    BEGIN
        ALTER TABLE dbo.Espacios ADD CONSTRAINT DF_Espacios_SumaHoras_Num DEFAULT 0 FOR SumaHoras;
    END
END
GO

-- Crear o modificar CalculaPorcentaje (Debe ser DECIMAL)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Espacios' AND COLUMN_NAME = 'CalculaPorcentaje')
BEGIN
    ALTER TABLE dbo.Espacios ADD CalculaPorcentaje DECIMAL(5,2) NOT NULL CONSTRAINT DF_Espacios_Calcula_Num DEFAULT 0;
END
ELSE
BEGIN
    ALTER TABLE dbo.Espacios ALTER COLUMN CalculaPorcentaje DECIMAL(5,2) NOT NULL;
    IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('dbo.Espacios') AND name = 'DF_Espacios_Calcula_Num')
    BEGIN
        ALTER TABLE dbo.Espacios ADD CONSTRAINT DF_Espacios_Calcula_Num DEFAULT 0 FOR CalculaPorcentaje;
    END
END
GO

-- Crear la columna Activo (Si no existe)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Espacios' AND COLUMN_NAME = 'Activo')
BEGIN
    ALTER TABLE dbo.Espacios ADD Activo BIT NOT NULL CONSTRAINT DF_Espacios_Activo DEFAULT 1;
END
GO

/* ============================================================================
      LIMPIEZA DE REGISTROS Y CARGA INICIAL
   ============================================================================ */
-- Eliminar cualquier fila extra (IDs mayores a 4) generada por error
DELETE FROM dbo.Espacios WHERE EspacioId > 4;
GO

-- Si la tabla está totalmente vacía, insertamos los 4 espacios base
IF NOT EXISTS (SELECT 1 FROM dbo.Espacios)
BEGIN
    INSERT INTO dbo.Espacios (Descripcion, SumaHoras, CalculaPorcentaje, Activo)
    VALUES 
        ('Esp. de Form. General', 0, 0, 1),
        ('Esp. de Form. Fundamentos', 0, 0, 1),
        ('Esp. de Form. Específica', 0, 0, 1),
        ('Esp. de Form. Prácticas Profesionalizante', 0, 0, 1);
END
ELSE
BEGIN
    -- Si los registros ya existen, reseteamos sus horas y porcentajes a 0
    UPDATE dbo.Espacios SET SumaHoras = 0, CalculaPorcentaje = 0;
END
GO

/* ============================================================================
      VERIFICACIÓN 
   ============================================================================ */
SELECT EspacioId, Descripcion, SumaHoras, CalculaPorcentaje, Activo 
FROM dbo.Espacios;
GO