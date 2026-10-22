--****************
-- ACTUALIZACIÓN DE FERNANDEZ FRANCO DANIEL
--01/06/2026
--EQUIPO 5
--si la base es la nueva, seguro ya existe
--****************
USE instituto_db;

GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Materias') AND name = 'FinalPromocion')
BEGIN
    ALTER TABLE Materias
    ADD FinalPromocion CHAR(1) DEFAULT 'F' NOT NULL
    CONSTRAINT CK_Materias_FinalPromocion CHECK (FinalPromocion IN('F','P'));
END
