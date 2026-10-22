USE [instituto_db];
GO

CREATE OR ALTER PROCEDURE dbo.SP_GenerarMesasFinales
    @AnioLectivoId INT,
    @LlamadoId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @AnioLectivoId <= 0
        THROW 50001, 'AnioLectivoId debe ser mayor a 0.', 1;

    IF @LlamadoId <= 0
        THROW 50002, 'LlamadoId debe ser mayor a 0.', 1;

    INSERT INTO dbo.MesasFinales
    (
        CarreraId,
        MateriaId,
        CursoMateriaId,
        LlamadoId,
        CicloLectivoId,
        FinalEstadoId
    )
    SELECT
        AC.CarreraId,
        CM.MateriaId,
        CM.CursoMateriaId,
        @LlamadoId,
        @AnioLectivoId,
        1
    FROM dbo.CursoMaterias CM
    INNER JOIN dbo.Cursos C
        ON C.CursoId = CM.CursoId
    INNER JOIN dbo.AniosCarreras AC
        ON AC.AnioCarreraId = C.AnioCarreraId
    INNER JOIN dbo.Carreras CA
        ON CA.CarreraId = AC.CarreraId
    WHERE CM.Activo = 1
      AND C.Activo = 1
      AND CA.CarreraEstadoId = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.MesasFinales MF
          WHERE MF.CursoMateriaId = CM.CursoMateriaId
            AND MF.CicloLectivoId = @AnioLectivoId
            AND MF.LlamadoId = @LlamadoId
      );

    SELECT @@ROWCOUNT AS MesasGeneradas;
END;
GO
