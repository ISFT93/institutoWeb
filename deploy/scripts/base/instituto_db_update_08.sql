USE instituto_db;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('cursos') AND name = 'CodigoBloque')
BEGIN
    ALTER TABLE cursos ADD [CodigoBloque] [varchar](10) NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CursoMaterias') AND name = 'CodigoBloque')
BEGIN
    ALTER TABLE CursoMaterias ADD CodigoBloque varchar(10)
END
