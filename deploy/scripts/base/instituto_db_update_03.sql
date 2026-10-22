--****************
-- ACTUALIZACIÓN DE FERNANDEZ FRANCO DANIEL
--19/06/2026
--EQUIPO 5
--****************
USE instituto_db;

GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Carreras') AND name = 'CantidadCorrelativas')
BEGIN
    ALTER TABLE Carreras ADD CantidadCorrelativas INT DEFAULT 0 NOT NULL;
END
