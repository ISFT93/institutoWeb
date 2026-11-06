USE instituto_db
GO

IF NOT EXISTS (SELECT 1 FROM Turnos WHERE TurnoId = 1)
BEGIN
    SET IDENTITY_INSERT Turnos ON;
    INSERT INTO Turnos (TurnoId, Descripcion) VALUES (1, 'Marzo');
    SET IDENTITY_INSERT Turnos OFF;
END
GO

IF NOT EXISTS (SELECT 1 FROM Turnos WHERE TurnoId = 2)
BEGIN
    SET IDENTITY_INSERT Turnos ON;
    INSERT INTO Turnos (TurnoId, Descripcion) VALUES (2, 'Julio');
    SET IDENTITY_INSERT Turnos OFF;
END
GO

IF NOT EXISTS (SELECT 1 FROM Turnos WHERE TurnoId = 3)
BEGIN
    SET IDENTITY_INSERT Turnos ON;
    INSERT INTO Turnos (TurnoId, Descripcion) VALUES (3, 'Diciembre');
    SET IDENTITY_INSERT Turnos OFF;
END
GO

IF NOT EXISTS (SELECT 1 FROM Turnos WHERE TurnoId = 4)
BEGIN
    SET IDENTITY_INSERT Turnos ON;
    INSERT INTO Turnos (TurnoId, Descripcion) VALUES (4, 'Especial');
    SET IDENTITY_INSERT Turnos OFF;
END
GO

IF NOT EXISTS (SELECT 1 FROM Llamados WHERE LlamadoId = 1)
BEGIN
    SET IDENTITY_INSERT Llamados ON;
    INSERT INTO Llamados (LlamadoId, Descripcion) VALUES (1, '1° llamado');
    SET IDENTITY_INSERT Llamados OFF;
END
GO

IF NOT EXISTS (SELECT 1 FROM Llamados WHERE LlamadoId = 2)
BEGIN
    SET IDENTITY_INSERT Llamados ON;
    INSERT INTO Llamados (LlamadoId, Descripcion) VALUES (2, '2° llamado');
    SET IDENTITY_INSERT Llamados OFF;
END
GO

IF NOT EXISTS (SELECT 1 FROM Llamados WHERE LlamadoId = 3)
BEGIN
    SET IDENTITY_INSERT Llamados ON;
    INSERT INTO Llamados (LlamadoId, Descripcion) VALUES (3, 'Fecha unica');
    SET IDENTITY_INSERT Llamados OFF;
END
GO
