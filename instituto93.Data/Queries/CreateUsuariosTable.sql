-- Usuarios solo guarda credenciales. Los datos personales viven en dbo.Alumnos.
-- Script idempotente: crea la tabla o migra el esquema anterior (con Nombre, Dni, Email, etc.).

IF OBJECT_ID(N'dbo.Alumnos', N'U') IS NULL
    THROW 50000, 'La tabla dbo.Alumnos debe existir antes de crear dbo.Usuarios.', 1;
GO

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        Id INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_Usuarios PRIMARY KEY,
        AlumnoId INT NOT NULL,
        Password NVARCHAR(512) NOT NULL,
        Activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
        CONSTRAINT UQ_Usuarios_AlumnoId UNIQUE (AlumnoId),
        CONSTRAINT FK_Usuarios_Alumnos
            FOREIGN KEY (AlumnoId) REFERENCES dbo.Alumnos (AlumnoId)
    );
END;
GO

IF COL_LENGTH(N'dbo.Usuarios', N'AlumnoId') IS NULL
    ALTER TABLE dbo.Usuarios ADD AlumnoId INT NULL;
GO

IF COL_LENGTH(N'dbo.Usuarios', N'Dni') IS NOT NULL
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;

    EXEC (N'
        UPDATE u
        SET AlumnoId = a.AlumnoId
        FROM dbo.Usuarios u
        JOIN dbo.Alumnos a
            ON REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(a.NumeroDocumento)), ''.'', ''''), '' '', ''''), ''-'', '''')
             = REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(u.Dni)), ''.'', ''''), '' '', ''''), ''-'', '''')
        WHERE u.AlumnoId IS NULL;');

    -- Usuarios sin alumno asociado no pueden existir en el nuevo modelo.
    EXEC (N'DELETE FROM dbo.Usuarios WHERE AlumnoId IS NULL;');

    IF OBJECT_ID(N'dbo.FK_Usuarios_Localidades', N'F') IS NOT NULL
        EXEC (N'ALTER TABLE dbo.Usuarios DROP CONSTRAINT FK_Usuarios_Localidades;');
    IF OBJECT_ID(N'dbo.UQ_Usuarios_Email', N'UQ') IS NOT NULL
        EXEC (N'ALTER TABLE dbo.Usuarios DROP CONSTRAINT UQ_Usuarios_Email;');
    IF OBJECT_ID(N'dbo.UQ_Usuarios_Dni', N'UQ') IS NOT NULL
        EXEC (N'ALTER TABLE dbo.Usuarios DROP CONSTRAINT UQ_Usuarios_Dni;');

    EXEC (N'ALTER TABLE dbo.Usuarios DROP COLUMN Nombre, Apellido, FechaNacimiento, Email, Dni, Telefono, Direccion, LocalidadId;');
    EXEC (N'ALTER TABLE dbo.Usuarios ALTER COLUMN AlumnoId INT NOT NULL;');
    EXEC (N'ALTER TABLE dbo.Usuarios ADD
        CONSTRAINT UQ_Usuarios_AlumnoId UNIQUE (AlumnoId),
        CONSTRAINT FK_Usuarios_Alumnos FOREIGN KEY (AlumnoId) REFERENCES dbo.Alumnos (AlumnoId);');

    COMMIT TRANSACTION;
END;
GO
