-- Roles de acceso de los usuarios (dbo.Usuarios.RolId).
-- Script idempotente. Debe ejecutarse antes de 02-CreateUsuariosTable.sql.

IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles
    (
        Id INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_Roles PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL,
        Activo BIT NOT NULL CONSTRAINT DF_Roles_Activo DEFAULT (1),
        CONSTRAINT UQ_Roles_Nombre UNIQUE (Nombre)
    );
END;
GO

INSERT INTO dbo.Roles (Nombre)
SELECT v.Nombre
FROM (VALUES (N'Alumno'), (N'Docente')) AS v (Nombre)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Roles r WHERE r.Nombre = v.Nombre);
GO
