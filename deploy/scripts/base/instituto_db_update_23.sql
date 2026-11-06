USE [instituto_db];
GO

ALTER PROCEDURE dbo.SP_CargaCursadas
    @AnioLectivoId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    INSERT INTO dbo.Cursadas
    (
        CursoMateriaId,
        AnioLectivo
    )
    SELECT
        CM.CursoMateriaId,
        @AnioLectivoId
    FROM dbo.CursoMaterias CM
    INNER JOIN dbo.Cursos CUR
        ON CM.CursoId = CUR.CursoId
    INNER JOIN dbo.AniosCarreras AC
        ON CUR.AnioCarreraId = AC.AnioCarreraId
    INNER JOIN dbo.Carreras CAR
        ON AC.CarreraId = CAR.CarreraId
    WHERE CAR.CarreraEstadoId = 1
      AND CUR.Activo = 1
      AND CM.Activo = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.Cursadas C
          WHERE C.CursoMateriaId = CM.CursoMateriaId
            AND C.AnioLectivo = @AnioLectivoId
      );

    SELECT @@ROWCOUNT AS CursadasGeneradas;
END;
GO
