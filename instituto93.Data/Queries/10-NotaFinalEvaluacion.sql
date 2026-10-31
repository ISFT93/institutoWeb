-- Garantiza que una cursada tenga como máximo una evaluación de tipo Nota final.
-- Requiere las tablas dbo.Evaluaciones y dbo.TiposEvaluacion.

USE instituto_db8;
GO

IF OBJECT_ID(N'dbo.Evaluaciones', N'U') IS NULL
    THROW 50000, 'La tabla dbo.Evaluaciones debe existir antes de aplicar esta regla.', 1;
GO

IF OBJECT_ID(N'dbo.TiposEvaluacion', N'U') IS NULL
    THROW 50000, 'La tabla dbo.TiposEvaluacion debe existir antes de aplicar esta regla.', 1;
GO

CREATE OR ALTER TRIGGER dbo.TR_Evaluaciones_UnaNotaFinalPorCursada
ON dbo.Evaluaciones
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.Evaluaciones e
        INNER JOIN dbo.TiposEvaluacion t
            ON t.TipoEvaluacionId = e.TipoEvaluacionId
        INNER JOIN inserted i
            ON i.CursadaId = e.CursadaId
        WHERE t.Codigo = N'NOTA_FINAL'
        GROUP BY e.CursadaId
        HAVING COUNT(*) > 1
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51001, 'Una cursada no puede tener más de una evaluación de tipo Nota final.', 1;
    END;
END;
GO
