IF OBJECT_ID(N'dbo.Localidades', N'U') IS NULL
    THROW 50000, 'La tabla dbo.Localidades debe existir antes de crear dbo.Usuarios.', 1;
GO

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        Id INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_Usuarios PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Apellido NVARCHAR(100) NOT NULL,
        FechaNacimiento DATETIME2 NOT NULL,
        Email NVARCHAR(256) NOT NULL,
        Password NVARCHAR(512) NOT NULL,
        Dni NVARCHAR(30) NOT NULL,
        Telefono NVARCHAR(50) NOT NULL,
        Direccion NVARCHAR(256) NOT NULL,
        LocalidadId INT NOT NULL,
        Activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
        CONSTRAINT UQ_Usuarios_Email UNIQUE (Email),
        CONSTRAINT UQ_Usuarios_Dni UNIQUE (Dni),
        CONSTRAINT FK_Usuarios_Localidades
            FOREIGN KEY (LocalidadId) REFERENCES dbo.Localidades (id)
    );
END;
GO
