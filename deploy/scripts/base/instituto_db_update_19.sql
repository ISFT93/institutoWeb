USE instituto_db;
GO

DELETE FROM Cargos;
GO



INSERT INTO Cargos ( Descripcion, Activo, TipoAsignacionId, TipoAplicacionId, CargaHoraria) 
VALUES 
( 'DIRECTIVO', 1, 1, 1, 20),
( 'REGENTE', 1, 1, 1, 20),
( 'SECRETARIO', 1, 1, 1, 20),
( 'PROFESOR', 1, 2, 1, 20),
( 'BIBLIOTECARIO', 1, 1, 1, 20),
( 'JEFE DE AREA MEDIO CARGO', 1, 1, 1, 10),
( 'PERSONAL AUXILIAR', 1, 1, 1, 20)

GO

