USE instituto_db
GO

IF NOT EXISTS (
	SELECT 1
    FROM sys.columns
    WHERE Name = N'CarreraID'
    AND Object_ID = Object_ID(N'LibroActas')
)
BEGIN
    ALTER TABLE LibroActas ADD CarreraID INT;
END
GO

IF NOT EXISTS (
	SELECT 1
	FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	WHERE TABLE_NAME = 'LibroActas'
	AND CONSTRAINT_NAME = 'LimitarFolios'
)
BEGIN
	ALTER TABLE LibroActas ADD CONSTRAINT LimitarFolios CHECK (FolioNumero <= FolioMaximo)
END
GO

IF NOT EXISTS (
	SELECT 1
	FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	WHERE TABLE_NAME = 'LibroActas'
	AND CONSTRAINT_NAME = 'FK_LibroActas_Carrera'
)
BEGIN
	ALTER TABLE LibroActas ADD CONSTRAINT FK_LibroActas_Carrera FOREIGN KEY (CarreraID) REFERENCES Carreras(CarreraID)
END
GO

CREATE OR ALTER PROCEDURE AgregarNumeroLibro
    @TipoLibroID INT,
    @LibroNumero INT,
	@CarreraID INT,
	@FolioMaximo INT,
	@FechaAlta DATE
AS
BEGIN
    INSERT INTO LibroActas 
	(TipoLibroId, LibroNumero, CarreraID, FolioNumero, FolioMaximo, FechaAlta, Activo) 
	VALUES 
	(@TipoLibroID, @LibroNumero, @CarreraID, 0, @FolioMaximo, @FechaAlta, 1)
END
GO

IF NOT EXISTS (SELECT * FROM TipoLibros WHERE Descripcion = 'Libro de Matriz') BEGIN
	INSERT INTO TipoLibros VALUES ('Libro de Matriz') END
GO

IF NOT EXISTS (SELECT * FROM TipoLibros WHERE Descripcion = 'Libro de Acta de Examenes') BEGIN
	INSERT INTO TipoLibros VALUES ('Libro de Acta de Examenes') END
GO

IF EXISTS (SELECT * FROM TipoLibros WHERE Descripcion = 'Libro de Certificado Analitico') BEGIN
	UPDATE TipoLibros SET Descripcion = 'Libro de Toma de Posición' WHERE Descripcion = 'Libro de Certificado Analitico' END
GO

IF NOT EXISTS (SELECT * FROM TipoLibros WHERE Descripcion = 'Libro de Toma de Posición') BEGIN
	INSERT INTO TipoLibros (Descripcion) VALUES ('Libro de Toma de Posición') END
GO

DECLARE @LibroTomaPosicionId INT;
SELECT @LibroTomaPosicionId = TipoLibroId FROM TipoLibros WHERE Descripcion = 'Libro de Toma de Posición';

UPDATE LibroActas SET CarreraID = NULL WHERE TipoLibroId = @LibroTomaPosicionId

;WITH UltimoLibro AS (SELECT MAX(LibroNumero) AS Ultimo FROM LibroActas WHERE TipoLibroId = @LibroTomaPosicionId)
	UPDATE LA SET FolioNumero = LA.FolioMaximo FROM LibroActas LA CROSS JOIN UltimoLibro UL WHERE LA.TipoLibroId = @LibroTomaPosicionId AND LA.LibroNumero <> UL.Ultimo;
