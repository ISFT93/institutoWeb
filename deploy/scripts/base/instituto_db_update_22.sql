USE [instituto_db];
GO

IF COL_LENGTH('dbo.MesasFinales', 'CursoMateriaId') IS NULL
BEGIN
    ALTER TABLE dbo.MesasFinales
    ADD CursoMateriaId INT NULL;
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_MesasFinales_CursoMaterias'
      AND parent_object_id = OBJECT_ID('dbo.MesasFinales')
)
BEGIN
    ALTER TABLE dbo.MesasFinales
    ADD CONSTRAINT FK_MesasFinales_CursoMaterias
        FOREIGN KEY (CursoMateriaId)
        REFERENCES dbo.CursoMaterias(CursoMateriaId);
END;
GO

GO

;WITH Candidatos AS
(
    SELECT
        MF.MesaFinalId,
        MIN(CM.CursoMateriaId) AS CursoMateriaId,
        COUNT(DISTINCT CM.CursoMateriaId) AS CantidadCandidatos
    FROM dbo.MesasFinales MF
    INNER JOIN dbo.CursoMaterias CM
        ON CM.MateriaId = MF.MateriaId
    INNER JOIN dbo.Cursos C
        ON C.CursoId = CM.CursoId
    INNER JOIN dbo.AniosCarreras AC
        ON AC.AnioCarreraId = C.AnioCarreraId
    WHERE MF.CursoMateriaId IS NULL
      AND AC.CarreraId = MF.CarreraId
      AND CM.Activo = 1
      AND C.Activo = 1
    GROUP BY MF.MesaFinalId
)
UPDATE MF
SET MF.CursoMateriaId = C.CursoMateriaId
FROM dbo.MesasFinales MF
INNER JOIN Candidatos C
    ON C.MesaFinalId = MF.MesaFinalId
WHERE C.CantidadCandidatos = 1;
GO

SELECT @@ROWCOUNT AS MesasActualizadas;
GO

GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'UX_MesasFinales_CursoMateria_Ciclo_Llamado'
      AND object_id = OBJECT_ID('dbo.MesasFinales')
)
BEGIN
    CREATE UNIQUE INDEX UX_MesasFinales_CursoMateria_Ciclo_Llamado
    ON dbo.MesasFinales
    (
        CursoMateriaId,
        CicloLectivoId,
        LlamadoId
    )
    WHERE CursoMateriaId IS NOT NULL
      AND CicloLectivoId IS NOT NULL
      AND LlamadoId IS NOT NULL;
END;

GO
IF OBJECT_ID('dbo.MesaAlumno', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.MesaAlumno
    (
        MesaAlumnoId INT IDENTITY(1,1) NOT NULL,
        MesaFinalId INT NOT NULL,
        CursadaAlumnoCarreraId INT NOT NULL,
        AlumnoId INT NULL,

        Nota DECIMAL(4,2) NULL,
        NotaLetra NVARCHAR(50) NULL,
        NotaDefinitiva DECIMAL(4,2) NULL,

        Estado NVARCHAR(30) NOT NULL
            CONSTRAINT DF_MesaAlumno_Estado
            DEFAULT (N'Pendiente'),

        Fecha DATETIME2(0) NOT NULL
            CONSTRAINT DF_MesaAlumno_Fecha
            DEFAULT (SYSDATETIME()),

        CONSTRAINT PK_MesaAlumno
            PRIMARY KEY (MesaAlumnoId),

        CONSTRAINT FK_MesaAlumno_MesasFinales
            FOREIGN KEY (MesaFinalId)
            REFERENCES dbo.MesasFinales(MesaFinalId),

        CONSTRAINT FK_MesaAlumno_CursadaAlumnoCarreras
            FOREIGN KEY (CursadaAlumnoCarreraId)
            REFERENCES dbo.CursadaAlumnoCarreras(CursadaAlumnoCarreraId),

        CONSTRAINT UQ_MesaAlumno_Mesa_CursadaAlumnoCarrera
            UNIQUE (MesaFinalId, CursadaAlumnoCarreraId)
    );
END;
GO
GO

ALTER TABLE dbo.MesaAlumno
ADD CONSTRAINT FK_MesaAlumno_Alumno
    FOREIGN KEY (AlumnoId)
    REFERENCES dbo.Alumnos(AlumnoId);
GO



-- MesasFinales.MateriaId se conserva: la usa SP_GenerarMesasFinales (update_24).
-- (El DROP COLUMN original fallaba por la FK autogenerada sobre MateriaId.)
GO

GO

IF COL_LENGTH('dbo.AlumnosCarreras', 'LibroActaId') IS NULL
BEGIN
    ALTER TABLE dbo.AlumnosCarreras
    ADD LibroActaId INT NULL;
END;
GO

IF COL_LENGTH('dbo.AlumnosCarreras', 'NroFolio') IS NULL
BEGIN
    ALTER TABLE dbo.AlumnosCarreras
    ADD NroFolio CHAR(3) NULL;
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_AlumnosCarreras_LibroActas'
      AND parent_object_id = OBJECT_ID('dbo.AlumnosCarreras')
)
BEGIN
    ALTER TABLE dbo.AlumnosCarreras
    ADD CONSTRAINT FK_AlumnosCarreras_LibroActas
        FOREIGN KEY (LibroActaId)
        REFERENCES dbo.LibroActas(LibroActaId);
END;
GO
