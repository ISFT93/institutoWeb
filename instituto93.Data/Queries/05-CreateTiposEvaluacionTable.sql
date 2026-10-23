-- Catálogo de tipos para las evaluaciones de una cursada (parciales, recuperatorios, etc.).
-- Se mantiene separado de dbo.TipoExamen, utilizado por el dominio de exámenes/mesas finales.
-- Script idempotente.

IF OBJECT_ID(N'dbo.TiposEvaluacion', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TiposEvaluacion
    (
        TipoEvaluacionId INT IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_TiposEvaluacion PRIMARY KEY,
        Codigo NVARCHAR(30) NOT NULL,
        Descripcion NVARCHAR(100) NOT NULL,
        Activo BIT NOT NULL
            CONSTRAINT DF_TiposEvaluacion_Activo DEFAULT (1),
        CONSTRAINT UQ_TiposEvaluacion_Codigo UNIQUE (Codigo)
    );
END;
GO

MERGE dbo.TiposEvaluacion AS destino
USING
(
    VALUES
        (N'PARCIAL', N'Parcial'),
        (N'RECUPERATORIO', N'Recuperatorio'),
        (N'TRABAJO_PRACTICO', N'Trabajo práctico'),
        (N'INTEGRADOR', N'Integrador')
) AS origen (Codigo, Descripcion)
ON destino.Codigo = origen.Codigo
WHEN MATCHED THEN
    UPDATE SET
        destino.Descripcion = origen.Descripcion,
        destino.Activo = 1
WHEN NOT MATCHED THEN
    INSERT (Codigo, Descripcion, Activo)
    VALUES (origen.Codigo, origen.Descripcion, 1);
GO
