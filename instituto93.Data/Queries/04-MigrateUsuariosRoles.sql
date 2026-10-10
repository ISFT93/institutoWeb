-- Migra una dbo.Usuarios existente (solo alumnos) al modelo con roles, sin dropear la tabla.
-- Agrega RolId (los usuarios actuales quedan con rol Alumno), hace AlumnoId opcional y agrega ProfesorId.
-- Script idempotente. Requiere dbo.Roles (01), dbo.Usuarios (02) y dbo.Personal.

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
    THROW 50000, 'La tabla dbo.Usuarios debe existir antes de migrarla.', 1;
GO

IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
    THROW 50000, 'La tabla dbo.Roles debe existir antes de migrar dbo.Usuarios (01-CreateRolesTable.sql).', 1;
GO

IF OBJECT_ID(N'dbo.Personal', N'U') IS NULL
    THROW 50000, 'La tabla dbo.Personal debe existir antes de migrar dbo.Usuarios.', 1;
GO

SET QUOTED_IDENTIFIER ON; -- requerido por los índices filtrados (sqlcmd lo deja en OFF por defecto)
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF COL_LENGTH(N'dbo.Usuarios', N'RolId') IS NULL
BEGIN
    EXEC (N'ALTER TABLE dbo.Usuarios ADD RolId INT NULL;');
    EXEC (N'
        UPDATE dbo.Usuarios
        SET RolId = (SELECT Id FROM dbo.Roles WHERE Nombre = N''Alumno'');');
    EXEC (N'ALTER TABLE dbo.Usuarios ALTER COLUMN RolId INT NOT NULL;');
    EXEC (N'ALTER TABLE dbo.Usuarios ADD CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (RolId) REFERENCES dbo.Roles (Id);');
END;

IF COL_LENGTH(N'dbo.Usuarios', N'ProfesorId') IS NULL
BEGIN
    EXEC (N'ALTER TABLE dbo.Usuarios ADD ProfesorId INT NULL;');
    EXEC (N'ALTER TABLE dbo.Usuarios ADD CONSTRAINT FK_Usuarios_Profesores FOREIGN KEY (ProfesorId) REFERENCES dbo.Personal (PersonalId);');
END;

IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = N'UQ_Usuarios_AlumnoId' AND parent_object_id = OBJECT_ID(N'dbo.Usuarios'))
    EXEC (N'ALTER TABLE dbo.Usuarios DROP CONSTRAINT UQ_Usuarios_AlumnoId;');

IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.Usuarios') AND name = N'AlumnoId' AND is_nullable = 0)
    EXEC (N'ALTER TABLE dbo.Usuarios ALTER COLUMN AlumnoId INT NULL;');

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Usuarios_AlumnoId' AND object_id = OBJECT_ID(N'dbo.Usuarios'))
    EXEC (N'CREATE UNIQUE INDEX UX_Usuarios_AlumnoId ON dbo.Usuarios (AlumnoId) WHERE AlumnoId IS NOT NULL;');

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Usuarios_ProfesorId' AND object_id = OBJECT_ID(N'dbo.Usuarios'))
    EXEC (N'CREATE UNIQUE INDEX UX_Usuarios_ProfesorId ON dbo.Usuarios (ProfesorId) WHERE ProfesorId IS NOT NULL;');

IF OBJECT_ID(N'dbo.CK_Usuarios_AlumnoOProfesor', N'C') IS NULL
    EXEC (N'ALTER TABLE dbo.Usuarios ADD CONSTRAINT CK_Usuarios_AlumnoOProfesor
        CHECK ((AlumnoId IS NOT NULL AND ProfesorId IS NULL) OR (AlumnoId IS NULL AND ProfesorId IS NOT NULL));');

COMMIT TRANSACTION;
GO
