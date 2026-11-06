USE instituto_db;

go

INSERT INTO Modulos (Descripcion)
SELECT Descripcion
FROM (VALUES 
    ('18:30 - 19:30'),
    ('19:30 - 20:30'),
    ('20:40 - 21:40'),
    ('21:40 - 22:40')
) AS Datos(Descripcion)
WHERE NOT EXISTS (
    SELECT 1 FROM Modulos m WHERE m.Descripcion = Datos.Descripcion
);

INSERT INTO Dias (Descripcion)
SELECT Descripcion
FROM (VALUES 
    ('Lunes'),
    ('Martes'),
    ('Miércoles'),
    ('Jueves'),
    ('Viernes')
) AS Datos(Descripcion)
WHERE NOT EXISTS (
    SELECT 1 FROM Dias d WHERE d.Descripcion = Datos.Descripcion
);