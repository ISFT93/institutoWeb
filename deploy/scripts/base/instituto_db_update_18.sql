/* ============================================================
   Script para base de datos nueva - Clasificación de Carreras
   Importante: puede ejecutarse más de una vez sin duplicar nada
   ============================================================ */

-- 1. Tabla familia
IF OBJECT_ID('dbo.familia', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.familia (
        Grupo_fami  INT IDENTITY(1,1) NOT NULL,
        descripción NVARCHAR(100)     NOT NULL,
        CONSTRAINT PK_familia PRIMARY KEY (Grupo_fami),
        CONSTRAINT UQ_familia_descripcion UNIQUE (descripción)
    );
END
GO

-- 2. Tabla variable
IF OBJECT_ID('dbo.variable', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[variable] (
        id_variable INT IDENTITY(1,1) NOT NULL,
        descripción NVARCHAR(100)     NOT NULL,
        Grupo_fami  INT               NOT NULL,
        activo      BIT               NOT NULL CONSTRAINT DF_variable_activo DEFAULT (1),
        CONSTRAINT PK_variable PRIMARY KEY (id_variable),
        CONSTRAINT FK_variable_familia FOREIGN KEY (Grupo_fami) REFERENCES dbo.familia (Grupo_fami),
        CONSTRAINT UQ_variable_familia_descripcion UNIQUE (Grupo_fami, descripción)
    );
END
GO

-- 3. Tabla clasificacion_carreras (almacena y compara la clasificación)
IF OBJECT_ID('dbo.clasificacion_carreras', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.clasificacion_carreras (
        id               INT IDENTITY(1,1) NOT NULL,
        descripcion      NVARCHAR(100)     NOT NULL,
        descripcionCorta NVARCHAR(100)     NULL,
        horas            INT               NULL,
        activo           BIT               NOT NULL CONSTRAINT DF_clasificacion_carreras_activo DEFAULT (1),
        CONSTRAINT PK_clasificacion_carreras PRIMARY KEY (id)
    );
END
GO

-- 4. Unique constraint para clasificacion_carreras
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints
               WHERE name = 'UQ_clasificacion_carreras_desc'
                 AND parent_object_id = OBJECT_ID('dbo.clasificacion_carreras'))
BEGIN
    ALTER TABLE dbo.clasificacion_carreras
        ADD CONSTRAINT UQ_clasificacion_carreras_desc UNIQUE (descripcion, descripcionCorta);
END
GO

-- 5. Carreras: columnas de texto y columnas Id (solo si no existen)
IF OBJECT_ID('dbo.Carreras', 'U') IS NOT NULL
BEGIN
    -- Columnas de texto
    IF COL_LENGTH('dbo.Carreras', 'RegimenDefecto')     IS NULL ALTER TABLE dbo.Carreras ADD RegimenDefecto     NVARCHAR(100) NULL;
    IF COL_LENGTH('dbo.Carreras', 'SectorActividad')    IS NULL ALTER TABLE dbo.Carreras ADD SectorActividad    NVARCHAR(100) NULL;
    IF COL_LENGTH('dbo.Carreras', 'FamiliaProfesional') IS NULL ALTER TABLE dbo.Carreras ADD FamiliaProfesional NVARCHAR(100) NULL;
    IF COL_LENGTH('dbo.Carreras', 'Variante')           IS NULL ALTER TABLE dbo.Carreras ADD Variante           NVARCHAR(100) NULL;
    IF COL_LENGTH('dbo.Carreras', 'Modalidad')          IS NULL ALTER TABLE dbo.Carreras ADD Modalidad          NVARCHAR(100) NULL;
 
    -- 6. Columnas Id con su clave foránea
    IF COL_LENGTH('dbo.Carreras', 'SectorActividadId')    IS NULL ALTER TABLE dbo.Carreras ADD SectorActividadId    INT NULL;
    IF COL_LENGTH('dbo.Carreras', 'FamiliaProfesionalId') IS NULL ALTER TABLE dbo.Carreras ADD FamiliaProfesionalId INT NULL;
    IF COL_LENGTH('dbo.Carreras', 'VarianteId')           IS NULL ALTER TABLE dbo.Carreras ADD VarianteId           INT NULL;
    IF COL_LENGTH('dbo.Carreras', 'ModalidadId')          IS NULL ALTER TABLE dbo.Carreras ADD ModalidadId          INT NULL;
    IF COL_LENGTH('dbo.Carreras', 'RegimenId')            IS NULL ALTER TABLE dbo.Carreras ADD RegimenId            INT NULL;
END
GO

-- Creación de Claves Foráneas para las columnas Id de Carreras


-- 7. Nº de Expediente -> Nº de Resolución
IF OBJECT_ID('dbo.Carreras', 'U') IS NOT NULL
BEGIN
    IF COL_LENGTH('dbo.Carreras', 'NumeroExpediente') IS NOT NULL
       AND COL_LENGTH('dbo.Carreras', 'NumeroResolucion') IS NULL
        EXEC sp_rename 'dbo.Carreras.NumeroExpediente', 'NumeroResolucion', 'COLUMN';

    IF EXISTS (SELECT 1 FROM sys.key_constraints
               WHERE name = 'UQ_NumeroExpediente' AND parent_object_id = OBJECT_ID('dbo.Carreras'))
       AND NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'UQ_NumeroResolucion')
        EXEC sp_rename 'dbo.UQ_NumeroExpediente', 'UQ_NumeroResolucion', 'OBJECT';
END
GO