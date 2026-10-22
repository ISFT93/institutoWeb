USE instituto_db
GO

IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Modulos' AND COLUMN_NAME = 'Turno'
)
BEGIN
    ALTER TABLE Modulos 
    ADD Turno VARCHAR(50) NULL;
    

END

go

UPDATE Modulos
SET Turno = 'Vespertino'
WHERE Turno IS NULL;



go 


IF NOT EXISTS (SELECT 1 FROM Modulos WHERE Descripcion = '13:00 - 14:00' AND Turno = 'Tarde')
BEGIN
    INSERT INTO Modulos (Descripcion, Turno) 
    VALUES ('13:00 - 14:00', 'Tarde');
END

IF NOT EXISTS (SELECT 1 FROM Modulos WHERE Descripcion = '14:00 - 15:00' AND Turno = 'Tarde')
BEGIN
    INSERT INTO Modulos (Descripcion, Turno) 
    VALUES ('14:00 - 15:00', 'Tarde');
END

IF NOT EXISTS (SELECT 1 FROM Modulos WHERE Descripcion = '15:00 - 16:00' AND Turno = 'Tarde')
BEGIN
    INSERT INTO Modulos (Descripcion, Turno) 
    VALUES ('15:00 - 16:00', 'Tarde');
END

IF NOT EXISTS (SELECT 1 FROM Modulos WHERE Descripcion = '16:00 - 17:00' AND Turno = 'Tarde')
BEGIN
    INSERT INTO Modulos (Descripcion, Turno) 
    VALUES ('16:00 - 17:00', 'Tarde');
END





IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Modulos' AND COLUMN_NAME = 'Activo'
)
BEGIN

    ALTER TABLE Modulos 
    ADD Activo BIT NOT NULL CONSTRAINT DF_Modulos_Activo DEFAULT (1);

end
