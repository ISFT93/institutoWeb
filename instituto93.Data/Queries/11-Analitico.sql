USE [instituto_db8];
GO

/*
    dbo.Analitico
    -------------
    Guarda el registro que se asienta en el libro de Certificado Analítico
    por cada cursada/final del alumno.

    Relaciones:
        CursadaAlumnoCarreraId -> dbo.CursadaAlumnoCarreras  (datos de la cursada)
        AlumnoCarreraId        -> dbo.AlumnosCarreras        (alumno, libro y folio)
        MesaFinalId            -> dbo.MesasFinales           (mesa final)
        MesaAlumnoId           -> dbo.MesaAlumno             (detalle de la mesa del alumno)

    Equivalencia con el diseño original:
        Analitico_nro         -> NroAnalitico
        CursadaId             -> CursadaAlumnoCarreraId
        AlumnoCarreraId       -> AlumnoCarreraId
        codigo_bloque_materia -> CodigoBloqueMateria
        AnioCicloLectivo      -> AnioCicloLectivo
        estado_cursada        -> EstadoCursada
        nro_libro             -> NroLibro
        nro_folio             -> NroFolio
        nota_mesa             -> NotaMesa
        nota_letra            -> NotaLetra
        fecha_mesa            -> FechaMesa
*/

IF OBJECT_ID('dbo.Analitico', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Analitico
    (
        AnaliticoId             INT IDENTITY(1,1) NOT NULL,

        -- Nro de orden del analítico
        NroAnalitico            INT NOT NULL,

        -- Claves de relación
        CursadaAlumnoCarreraId  INT NOT NULL,
        AlumnoCarreraId         INT NOT NULL,
        MesaFinalId             INT NULL,
        MesaAlumnoId            INT NULL,

        -- Datos de la materia / cursada
        CodigoBloqueMateria     VARCHAR(20) NULL,
        AnioCicloLectivo        INT NOT NULL,
        EstadoCursada           NVARCHAR(100) NULL,

        -- Datos de la mesa final
        NroLibro                INT NULL,
        NroFolio                INT NULL,
        NotaMesa                DECIMAL(4,2) NULL,
        NotaLetra               NVARCHAR(50) NULL,
        FechaMesa               DATETIME2(0) NULL,

        CONSTRAINT PK_Analitico
            PRIMARY KEY CLUSTERED (AnaliticoId),

        CONSTRAINT FK_Analitico_CursadaAlumnoCarreras
            FOREIGN KEY (CursadaAlumnoCarreraId)
            REFERENCES dbo.CursadaAlumnoCarreras (CursadaAlumnoCarreraId),

        CONSTRAINT FK_Analitico_AlumnosCarreras
            FOREIGN KEY (AlumnoCarreraId)
            REFERENCES dbo.AlumnosCarreras (AlumnoCarreraId),

        CONSTRAINT FK_Analitico_MesasFinales
            FOREIGN KEY (MesaFinalId)
            REFERENCES dbo.MesasFinales (MesaFinalId),

        CONSTRAINT FK_Analitico_MesaAlumno
            FOREIGN KEY (MesaAlumnoId)
            REFERENCES dbo.MesaAlumno (MesaAlumnoId)
    );
END;
GO

-- Permite guardar condición + establecimiento en la misma columna del analítico.
IF COL_LENGTH('dbo.Analitico', 'EstadoCursada') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Analitico
        ALTER COLUMN EstadoCursada NVARCHAR(100) NULL;
END;
GO

-- Índices sobre las claves foráneas (SQL Server no las indexa automáticamente)
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Analitico_CursadaAlumnoCarreraId'
      AND object_id = OBJECT_ID('dbo.Analitico')
)
BEGIN
    CREATE INDEX IX_Analitico_CursadaAlumnoCarreraId
        ON dbo.Analitico (CursadaAlumnoCarreraId);
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Analitico_AlumnoCarreraId'
      AND object_id = OBJECT_ID('dbo.Analitico')
)
BEGIN
    CREATE INDEX IX_Analitico_AlumnoCarreraId
        ON dbo.Analitico (AlumnoCarreraId);
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Analitico_MesaFinalId'
      AND object_id = OBJECT_ID('dbo.Analitico')
)
BEGIN
    CREATE INDEX IX_Analitico_MesaFinalId
        ON dbo.Analitico (MesaFinalId);
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Analitico_MesaAlumnoId'
      AND object_id = OBJECT_ID('dbo.Analitico')
)
BEGIN
    CREATE INDEX IX_Analitico_MesaAlumnoId
        ON dbo.Analitico (MesaAlumnoId);
END;
GO
