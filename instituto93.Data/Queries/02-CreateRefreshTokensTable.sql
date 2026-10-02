-- Refresh tokens opacos con rotación (RFC 9700 §4.14.2). Solo se guarda el hash SHA-256.
-- Todos los tokens emitidos a partir de un mismo login comparten FamilyId; si se reutiliza
-- un token ya rotado se revoca la familia completa.
-- Script idempotente. Requiere dbo.Usuarios (CreateUsuariosTable.sql).

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
    THROW 50000, 'La tabla dbo.Usuarios debe existir antes de crear dbo.RefreshTokens.', 1;
GO

IF OBJECT_ID(N'dbo.RefreshTokens', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RefreshTokens
    (
        Id INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_RefreshTokens PRIMARY KEY,
        UsuarioId INT NOT NULL,
        FamilyId UNIQUEIDENTIFIER NOT NULL,
        TokenHash VARBINARY(32) NOT NULL,
        CreatedAt DATETIME2(0) NOT NULL,
        ExpiresAt DATETIME2(0) NOT NULL,
        FamilyExpiresAt DATETIME2(0) NOT NULL,
        UsedAt DATETIME2(0) NULL,
        RevokedAt DATETIME2(0) NULL,
        ReplacedById INT NULL,
        CONSTRAINT UQ_RefreshTokens_TokenHash UNIQUE (TokenHash),
        CONSTRAINT FK_RefreshTokens_Usuarios
            FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios (Id) ON DELETE CASCADE,
        CONSTRAINT FK_RefreshTokens_ReplacedBy
            FOREIGN KEY (ReplacedById) REFERENCES dbo.RefreshTokens (Id)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_RefreshTokens_FamilyId' AND object_id = OBJECT_ID(N'dbo.RefreshTokens'))
    CREATE INDEX IX_RefreshTokens_FamilyId ON dbo.RefreshTokens (FamilyId);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_RefreshTokens_UsuarioId' AND object_id = OBJECT_ID(N'dbo.RefreshTokens'))
    CREATE INDEX IX_RefreshTokens_UsuarioId ON dbo.RefreshTokens (UsuarioId);
GO
