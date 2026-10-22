--si la base es la nueva, ya existen estas columnas
USE instituto_db
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CicloLectivo') AND name = 'FechaInscripcionSuperioresInicio')
BEGIN
    ALTER TABLE dbo.CicloLectivo
    ADD FechaInscripcionSuperioresInicio DATE NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CicloLectivo') AND name = 'FechaInscripcionSuperioresFinal')
BEGIN
    ALTER TABLE dbo.CicloLectivo
    ADD FechaInscripcionSuperioresFinal DATE NULL
END
GO
