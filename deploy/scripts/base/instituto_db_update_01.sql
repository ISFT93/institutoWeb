
--****************
-- ACTUALIZACION DE FERNANDEZ FRANCO DANIEL
--16/05/2026
--EQUIPO 5
--si corrio la base nueva ya existen estos campos y va a fallar, no seria un problema
--****************

USE instituto_db;

GO

/*************************COLUMNAS ADICIONALES*************************************/

--Creacion de una nueva columna para almacenar el codigo de bloque de la tabla Carreras
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Carreras') AND name = 'CarrerasCodigoBloque')
BEGIN
    ALTER TABLE Carreras ADD CarrerasCodigoBloque VARCHAR(20);
END

GO

--Creacion de una nueva columna para almacenar el codigo de bloque de la tabla AniosCarreras
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AniosCarreras') AND name = 'AniosCarrerasCodigoBloque')
BEGIN
    ALTER TABLE AniosCarreras ADD AniosCarrerasCodigoBloque VARCHAR(20);
END

GO

--Creacion de un campo CarreraId dentro de la tabla Materias.
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Materias') AND name = 'CarreraId')
BEGIN
    ALTER TABLE Materias ADD CarreraId INT;
END

GO

--Creacion de una nueva columna para almacenar el codigo de bloque de la tabla Materias
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Materias') AND name = 'MateriasCodigoBloque')
BEGIN
    ALTER TABLE Materias ADD MateriasCodigoBloque VARCHAR(20);
END

GO

/****************CREACION DE CODIGO PARA TABLA Carreras****************/

;WITH Codigos AS (
    SELECT 
        CarreraId,
        FORMAT(DENSE_RANK() OVER (ORDER BY CarreraId), '00') AS CarrerasCodigoBloque
    FROM Carreras
)
UPDATE CA
SET CA.CarrerasCodigoBloque = C.CarrerasCodigoBloque
FROM Carreras CA
INNER JOIN Codigos C ON CA.CarreraId = C.CarreraId;


GO

--Indice del nuevo campo CarrerasCodigoBloque
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'I_Carreras_C_CodigoBloque' AND object_id = OBJECT_ID('Carreras'))
BEGIN
    CREATE INDEX I_Carreras_C_CodigoBloque
    ON Carreras(CarrerasCodigoBloque);
END

GO

/****************CREACION DE CODIGO PARA TABLA AniosCarreras****************/


;WITH Codigos AS (
    SELECT 
        AnioCarreraId,
        FORMAT(DENSE_RANK() OVER (ORDER BY CarreraId), '00') + '' +
        FORMAT(ROW_NUMBER() OVER (PARTITION BY CarreraId ORDER BY AnioCarreraId), '0') AS AniosCarrerasCodigoBloque
    FROM AniosCarreras
)
UPDATE AC
SET AC.AniosCarrerasCodigoBloque = C.AniosCarrerasCodigoBloque
FROM AniosCarreras AC
INNER JOIN Codigos C ON AC.AnioCarreraId = C.AnioCarreraId;

GO

--Indice para AniosCarrerasCodigoBloque
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'I_AniosCarreras_AC_CodigoBloque' AND object_id = OBJECT_ID('AniosCarreras'))
BEGIN
    CREATE INDEX I_AniosCarreras_AC_CodigoBloque
    ON AniosCarreras(AniosCarrerasCodigoBloque);
END


GO

/****************CREACIÓN DE CODIGO DE BLOQUE PARA LA TABLA Materias****************/

--Actualizacion del campo CarreraId perteneciente a la tabla Materias
--con los ID pertenecientes de cada carrera.

--Sistemas
UPDATE Materias SET CarreraId = 1003 WHERE AnioCarreraId IN(2002,2003,2004);
GO
--Enfermeria
UPDATE Materias SET CarreraId = 1004 WHERE AnioCarreraId IN(2005,2006,2007);
GO
--Marketing
UPDATE Materias SET CarreraId = 2010 WHERE AnioCarreraId IN(3005,3006,3007);
GO
--Turismo
UPDATE Materias SET CarreraId = 3004 WHERE AnioCarreraId IN(4005,4006,4007);
GO
--Publica
UPDATE Materias SET CarreraId = 3005 WHERE AnioCarreraId IN(4008,4009,4010);
GO
--Contable
UPDATE Materias SET CarreraId = 3006 WHERE AnioCarreraId IN(4011,4012,4013);
GO

--Foreign Key para mantener integridad referencial entre Carreras y Materias
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Materias_Carreras' AND parent_object_id = OBJECT_ID('Materias'))
BEGIN
    ALTER TABLE Materias
    ADD CONSTRAINT FK_Materias_Carreras
    FOREIGN KEY (CarreraId) REFERENCES Carreras(CarreraId);
END

GO

;WITH Codigos AS (
	SELECT
    AC.AnioCarreraId,
    M.CarreraId,
    M.MateriaId,
    FORMAT(DENSE_RANK() OVER (ORDER BY AC.CarreraId), '00') + '' +
    FORMAT(DENSE_RANK() OVER (PARTITION BY AC.CarreraId ORDER BY AC.AnioCarreraId), '0') + '' +
    FORMAT(ROW_NUMBER() OVER (PARTITION BY AC.CarreraId, AC.AnioCarreraId ORDER BY M.MateriaId), '00') AS MateriasCodigoBloque
FROM AniosCarreras AC
LEFT JOIN Materias M ON AC.AnioCarreraId = M.AnioCarreraId
)
UPDATE M
SET M.MateriasCodigoBloque = C.MateriasCodigoBloque
FROM Materias M
INNER JOIN Codigos C ON M.MateriaId = C.MateriaId;

GO

--Indice para MateriasCodigoBloque
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'I_Materias_MateriasCodigo' AND object_id = OBJECT_ID('Materias'))
BEGIN
    CREATE INDEX I_Materias_MateriasCodigo
    ON Materias(MateriasCodigoBloque);
END


--Modificacion SP para que muestre el codigo de bloque en pantalla
--cuando es llamado desde el formulario que controla el asignado de correlatividades
/****** Object:  StoredProcedure [dbo].[SP_ListaMaterias]    Script Date: 16/05/2026 19:04:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[SP_ListaMaterias](
@MateriaID as int,
@CarreraID as int,
@msj as varchar(50) output
)
AS
BEGIN
BEGIN TRY
   -- Variable interna
	Declare @AnioCarrera As int

	create table #Temporal (
	MateriaCorrelativaId int)

	insert into #Temporal
	select MateriaCorrelativaId from Correlativas
	where MateriaId = @MateriaID


	SELECT @AnioCarrera = AnioCarrera FROM Materias as Mat
	INNER join AniosCarreras AS Ani on Mat.AnioCarreraId = Ani.AnioCarreraId
	WHERE Mat.MateriaId = @MateriaID

	select Mat.MateriaId, CONCAT(Mat.MateriasCodigoBloque, ' - ', Mat.Nombre) AS Materia from Materias as Mat
	INNER join AniosCarreras AS Ani on Mat.AnioCarreraId = Ani.AnioCarreraId INNER JOIN Carreras as C
	ON Ani.CarreraId = C.CarreraId
	WHERE Ani.AnioCarrera < @AnioCarrera AND Ani.CarreraId = @CarreraID AND
	Mat.MateriaId NOT IN (select MateriaCorrelativaId FROM #Temporal)
	ORDER BY Ani.AnioCarrera, Mat.Nombre

	drop table #Temporal
	SET @msj = 'La lista se genero correctamente.'
END TRY

BEGIN CATCH
	SET @msj = 'Ocurrio un Error: ' + ERROR_MESSAGE() + ' en la linea ' + CONVERT(nvarchar(255), ERROR_LINE()) + '.'
END CATCH
END
