USE instituto_db
GO

-- CREACION DE COLUMNA 'CargaHoraria'
IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE Name = N'CargaHoraria'
      AND Object_ID = Object_ID(N'Cargos')
)
BEGIN
    ALTER TABLE Cargos ADD CargaHoraria INT;
END
GO

-- CREACION DE SP 'InsertarCargo'
CREATE OR ALTER PROCEDURE InsertarCargo
    @Descripcion VARCHAR(50),
    @CargaHoraria INT,
    @TipoAsignacionId INT,
    @TipoAplicacionId INT
AS
BEGIN
    INSERT INTO Cargos
    (Descripcion, CargaHoraria, TipoAsignacionId, TipoAplicacionId, Activo)
    VALUES
    (@Descripcion, @CargaHoraria, @TipoAsignacionId, @TipoAplicacionId, 1)
END
GO

--CREACION Y ACTUALIZACION DE TIPOS DE APLICACIONES
IF EXISTS (SELECT * FROM TipoAplicacion WHERE Descripcion = 'Unico') BEGIN
	UPDATE TipoAplicacion SET Descripcion = '= 1' WHERE Descripcion = 'Unico' 
END

IF EXISTS (SELECT * FROM TipoAplicacion WHERE Descripcion = 'Repetible') BEGIN
	UPDATE TipoAplicacion SET Descripcion = '> 1' WHERE Descripcion = 'Repetible' 
END

IF NOT EXISTS (SELECT * FROM TipoAplicacion WHERE Descripcion = '= 1' ) BEGIN
	INSERT INTO TipoAplicacion (Descripcion, Detalle) VALUES ('= 1', 'El personal toma el cargo y no puede ser tomado x otro ni tomar otro cargo.') 
END

IF NOT EXISTS (SELECT * FROM TipoAplicacion WHERE Descripcion = '> 1' ) BEGIN
	INSERT INTO TipoAplicacion (Descripcion, Detalle) VALUES ('> 1', 'Puede Asignarse, ser asignado por otros y a la vez asignarse a otros cargos.')
END

IF EXISTS (SELECT * FROM TipoAplicacion WHERE Descripcion = '>= 1' OR Descripcion = 'Unico  Repetible' OR Descripcion = 'Exclusivo y Repetible') BEGIN
	UPDATE TipoAplicacion SET Descripcion = '> 1' WHERE Descripcion IN ('Unico  Repetible', 'Exclusivo y Repetible', '>= 1')
END
GO

UPDATE Cargos SET TipoAplicacionId = (SELECT TOP 1 TipoAplicacionId FROM TipoAplicacion WHERE Descripcion = '= 1' ORDER BY TipoAplicacionId ASC)
WHERE TipoAplicacionId IN (SELECT TipoAplicacionId FROM TipoAplicacion WHERE Descripcion = '= 1')

UPDATE Cargos SET TipoAplicacionId = (SELECT TOP 1 TipoAplicacionId FROM TipoAplicacion WHERE Descripcion = '> 1' ORDER BY TipoAplicacionId ASC)
WHERE TipoAplicacionId IN (SELECT TipoAplicacionId FROM TipoAplicacion WHERE Descripcion = '> 1')
GO

;WITH Duplicados AS (SELECT TipoAplicacionId, Descripcion, ROW_NUMBER() OVER (PARTITION BY Descripcion ORDER BY TipoAplicacionId) AS rn
    FROM TipoAplicacion WHERE Descripcion IN ('= 1', '> 1')) DELETE FROM Duplicados WHERE rn > 1;

DELETE FROM TipoAplicacion WHERE Descripcion NOT IN ('= 1', '> 1')
GO

IF NOT EXISTS (SELECT * FROM TipoAsignacion WHERE Descripcion = 'No Asignar') BEGIN
	INSERT INTO TipoAsignacion VALUES ('No Asignar') END

IF NOT EXISTS (SELECT * FROM TipoAsignacion WHERE Descripcion = 'Asignar a Materia') BEGIN
	INSERT INTO TipoAsignacion VALUES ('Asignar a Materia') END

IF NOT EXISTS (SELECT * FROM TipoAsignacion WHERE Descripcion = 'Asignar a Carrera') BEGIN
	INSERT INTO TipoAsignacion VALUES ('Asignar a Carrera') END
